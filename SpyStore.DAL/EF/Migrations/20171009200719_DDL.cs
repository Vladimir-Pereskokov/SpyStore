using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
namespace SpyStore.DAL.EF.Migrations
{
    public partial class DDL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           


            var sql = "CREATE FUNCTION Store.GetOrderTotal(@OrderId int) " +
                      "Returns Money WITH SCHEMABINDING " + 
                      "BEGIN " +
                      "Declare @Result Money; " +
                      "Select @Result = SUM([Quantity] * [UnitCost]) From Store.OrderDetails Where OrderID = @OrderID; " +
                      "Return @Result " +
                      "END";
            migrationBuilder.Sql(sql);


            sql = "CREATE Procedure Store.[PurtchaseOrdersInCart](" +
                "@customerId int, @orderId int OUT) AS " +
                "BEGIN " +
                "SET NOCOUNT ON; " +
                "DECLARE @TranNAME varchar(20); " +
                "SELECT @TranNAME = 'CommitOrder'; " +                                
                "BEGIN TRY " +
                "BEGIN TRANSACTION @TranNAME; " +
                "INSERT INTO Store.Orders (CustomerID, OrderDate, ShipDate) " +
                "VALUES (@customerId, GetDate(), GetDate()); " +
                "SET @orderId = SCOPE_IDENTITY(); " +
                "INSERT INTO Store.OrderDetails (OrderID, ProductID, Quantity, UnitCost) " +
                "SELECT @orderId, ProductId, Quantity, p.UnitPrice FROM Store.ShoppingCartRecords scr " +
                "INNER JOIN Store.Products p ON p.Id = scr.ProductId WHERE scr.CustomerId = @customerId; " +
                "COMMIT TRANSACTION @TranName; " +
                "END TRY " +                               
                "BEGIN CATCH " +
                "ROLLBACK TRANSACTION @TranName; " +                    
                "END CATCH " +
                "END;";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION [Store].GetOrderTotal;");
            migrationBuilder.Sql("DROP PROCEDURE [Store].PurtchaseOrdersInCart;");

        }
    }
}
