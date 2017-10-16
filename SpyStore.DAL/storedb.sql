IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF SCHEMA_ID(N'Store') IS NULL EXEC(N'CREATE SCHEMA [Store];');

GO

CREATE TABLE [Store].[Categories] (
    [Id] int NOT NULL IDENTITY,
    [CategoryName] nvarchar(50) NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Store].[Customers] (
    [Id] int NOT NULL IDENTITY,
    [EmailAddress] nvarchar(50) NOT NULL,
    [FullName] nvarchar(50) NULL,
    [Password] nvarchar(50) NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ Store].[Products] (
    [Id] int NOT NULL IDENTITY,
    [CategoryID] int NOT NULL,
    [Description] nvarchar(3800) NULL,
    [IsFeatured] bit NOT NULL,
    [ModelName] nvarchar(50) NULL,
    [ModelNumber] nvarchar(50) NULL,
    [ProductImage] nvarchar(150) NULL,
    [ProductImageThumb] nvarchar(150) NULL,
    [TimeStamp] rowversion NULL,
    [UnitCost] money NOT NULL,
    [UnitPrice] money NOT NULL,
    [UnitsInStock] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Categories_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [Store].[Categories] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Store].[Orders] (
    [Id] int NOT NULL IDENTITY,
    [CustomerID] int NOT NULL,
    [OrderDate] datetime NOT NULL DEFAULT (getdate()),
    [ShipDate] datetime NOT NULL DEFAULT (getdate()),
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Customers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [Store].[Customers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Store].[ShoppingCartRecords] (
    [Id] int NOT NULL IDENTITY,
    [CustomerID] int NOT NULL,
    [DateTimeCreated] datetime NULL DEFAULT (getdate()),
    [ProductID] int NOT NULL,
    [Quantity] int NOT NULL DEFAULT 1,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_ShoppingCartRecords] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ShoppingCartRecords_Customers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [Store].[Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ShoppingCartRecords_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [ Store].[Products] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Store].[OrderDetails] (
    [Id] int NOT NULL IDENTITY,
    [LineItemTotal] AS [Quantity]*[UnitCost],
    [OrderID] int NOT NULL,
    [ProductID] int NOT NULL,
    [Quantity] int NOT NULL,
    [TimeStamp] rowversion NULL,
    [UnitCost] money NOT NULL,
    CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderDetails_Orders_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Store].[Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderDetails_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [ Store].[Products] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Products_CategoryID] ON [ Store].[Products] ([CategoryID]);

GO

CREATE UNIQUE INDEX [IDX_EmailAddress] ON [Store].[Customers] ([EmailAddress]);

GO

CREATE INDEX [IX_OrderDetails_OrderID] ON [Store].[OrderDetails] ([OrderID]);

GO

CREATE INDEX [IX_OrderDetails_ProductID] ON [Store].[OrderDetails] ([ProductID]);

GO

CREATE INDEX [IX_Orders_CustomerID] ON [Store].[Orders] ([CustomerID]);

GO

CREATE INDEX [IX_ShoppingCartRecords_CustomerID] ON [Store].[ShoppingCartRecords] ([CustomerID]);

GO

CREATE INDEX [IX_ShoppingCartRecords_ProductID] ON [Store].[ShoppingCartRecords] ([ProductID]);

GO

CREATE UNIQUE INDEX [IDX_ShoppingCart] ON [Store].[ShoppingCartRecords] ([Id], [CustomerID], [ProductID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171008055607_Initial', N'2.0.0-rtm-26452');

GO

CREATE FUNCTION Store.GetOrderTotal(@OrderId int) Returns Money WITH SCHEMABINDING BEGIN Declare @Result Money; Select @Result = SUM([Quantity] * [UnitCost]) From Store.OrderDetails Where OrderID = @OrderID; Return @Result END

GO

CREATE Procedure Store.[PurtchaseOrdersInCart](@customerId int, @orderId int OUT) AS BEGIN SET NOCOUNT ON; DECLARE @TranNAME varchar(20); SELECT @TranNAME = 'CommitOrder'; BEGIN TRY BEGIN TRANSACTION @TranNAME; INSERT INTO Store.Orders (CustomerID, OrderDate, ShipDate) VALUES (@customerId, GetDate(), GetDate()); SET @orderId = SCOPE_IDENTITY(); INSERT INTO Store.OrderDetails (OrderID, ProductID, Quantity, UnitCost) SELECT @orderId, ProductId, Quantity, p.UnitPrice FROM Store.ShoppingCartRecords scr INNER JOIN Store.Products p ON p.Id = scr.ProductId WHERE scr.CustomerId = @customerId; COMMIT TRANSACTION @TranName; END TRY BEGIN CATCH ROLLBACK TRANSACTION @TranName; END CATCH END;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171009200719_DDL', N'2.0.0-rtm-26452');

GO

ALTER SCHEMA [Store] TRANSFER [ Store].[Products];

GO

ALTER TABLE [Store].[Orders] ADD [OrderTotal] AS Store.GetOrderTotal([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171016054929_FINAL', N'2.0.0-rtm-26452');

GO

