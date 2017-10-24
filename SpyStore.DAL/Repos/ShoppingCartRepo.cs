using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.ViewModels.Base;
using SpyStore.Models.ViewModels;

namespace SpyStore.DAL.Repos
{
    class ShoppingCartRepo : RepoBase<ShoppingCartRecord>, IShoppingCartRepo

    {
        private IProductRepo _productRepo;

        public ShoppingCartRepo(IProductRepo productRepo) : base() { _productRepo = productRepo; }
        public ShoppingCartRepo(DbContextOptions<StoreContext> options,
            IProductRepo productRepo) : base(options) { _productRepo = productRepo; }
        public override IEnumerable<ShoppingCartRecord> GetAll() =>
        table.OrderByDescending(c => c.DateTimeCreated);

        public override IEnumerable<ShoppingCartRecord> GetRange(int skip, int take) =>
            base.GetRange(table.OrderByDescending(r => r.DateTimeCreated), skip, take);



        




        #region "IShoppingCartRepo"
        public int Add(ShoppingCartRecord entity, int? quantityInStock, bool persist)
        {
            if (entity != null)
            {
                var existEnt = ((IShoppingCartRepo)this).Find(entity.CustomerID, entity.ProductID);
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
            table.FirstOrDefault(r => r.CustomerID == customerId && r.ProductID == productId);


        public CartRecordWithProductInfo GetShoppingCartRecord(int customerId, int productId)
        {
            throw new System.NotImplementedException();
        }

        public int Purchase(int customerId)
        {
            throw new System.NotImplementedException();
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
