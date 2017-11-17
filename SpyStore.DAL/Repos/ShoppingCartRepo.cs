using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.ViewModels.Base;
using SpyStore.Models.ViewModels;
using System.Data.SqlClient;
using System;


namespace SpyStore.DAL.Repos
{
    public class ShoppingCartRepo : RepoBase<ShoppingCartRecord>, IShoppingCartRepo

    {
        private IProductRepo _productRepo;

        public ShoppingCartRepo(IProductRepo productRepo) : base() { _productRepo = productRepo; }
        public ShoppingCartRepo(DbContextOptions<StoreContext> options,
            IProductRepo productRepo) : base(options) { _productRepo = productRepo; }
        public override IEnumerable<ShoppingCartRecord> GetAll() =>
        table.OrderByDescending(c => c.DateCreated);

        public override IEnumerable<ShoppingCartRecord> GetRange(int skip, int take) =>
            base.GetRange(table.OrderByDescending(r => r.DateCreated), skip, take);


        private CartRecordWithProductInfo GetRecord(int customerID, ShoppingCartRecord scr, Product p, Category c)
            => new CartRecordWithProductInfo
            {
                Id= scr.Id,
                DateCreated = scr.DateCreated,
                CustomerId = customerID,
                Quantity = scr.Quantity,
                ProductId = scr.ProductId,
                Description = p.Description,
                ModelName = p.ModelName,
                ModelNumber = p.ModelNumber,
                ProductImage = p.ProductImage,
                ProductImageLarge = p.ProductImageLarge,
                ProductImageThumb = p.ProductImageThumb,
                UnitPrice  = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                CategoryName = c.CategoryName,
                LineItemTotal = scr.Quantity * p.UnitPrice,
                TimeStamp = scr.TimeStamp 
            };

        




        #region "IShoppingCartRepo"
        public int Add(ShoppingCartRecord entity, int? quantityInStock, bool persist)
        {
            if (entity != null)
            {
                var existEnt = ((IShoppingCartRepo)this).Find(entity.CustomerId, entity.ProductId);
                if (existEnt == null)
                {
                    if (quantityInStock != null && entity.Quantity > quantityInStock)
                    {
                        throw new Exceptions.InvalidQuantityException(@"Can't add more product than available in stock");
                    }
                    else
                    {
                        return base.Add(entity, persist);
                    }
                }
                existEnt.Quantity += entity.Quantity;
                return existEnt.Quantity <= 0? Delete(existEnt, persist): Update(existEnt, quantityInStock, persist);
            }
            return 0;
        }

        public ShoppingCartRecord Find(int customerId, int productId) =>
            table.FirstOrDefault(r => r.CustomerId == customerId && r.ProductId == productId);


        public CartRecordWithProductInfo GetShoppingCartRecord(int customerId, int productId)
            => table.Where(r => r.CustomerId == customerId && r.ProductId == productId)
            .Include(c => c.Product)
            .ThenInclude(p => p.Category)            
            .Select(c => GetRecord(customerId, c, c.Product, c.Product.Category))
            .FirstOrDefault();
        public IEnumerable<CartRecordWithProductInfo> GetShoppingCartRecords(int customerId)
            => table.Where(r => r.CustomerId == customerId)
            .Include(r => r.Product)
            .ThenInclude(p => p.Category)
            .OrderBy(r => r.DateCreated)
            .Select(r => GetRecord(customerId, r, r.Product, r.Product.Category));



        public int Purchase(int customerId)
        {
            var customerIdparam = new SqlParameter("@customerId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Input ,
                Value  = customerId
            };
            var orderIdParam = new SqlParameter("@orderId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output 
            };
            try
            {
                base.Context().Database.ExecuteSqlCommand("EXEC [Store].[PurchaseItemsInCart] @customerId, @orderId", customerIdparam, orderIdParam);
                return (int)orderIdParam.Value;
                
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                return -1;                
            }
            
        }

        public int Update(ShoppingCartRecord entity, int? quantityInStock, bool persist = true)
        {
            if (entity == null)
                return 0;
            else
            {
                if (entity.Quantity <= 0)
                    return Delete(entity, persist);
                else if (entity.Quantity > quantityInStock)
                {
                    throw new Exceptions.InvalidQuantityException(@"Can't add more products than " +
                        "available in stock.");
                }
                else
                {
                    return base.Update(entity, persist);
                }
            }
        }
        #endregion
    }
}
