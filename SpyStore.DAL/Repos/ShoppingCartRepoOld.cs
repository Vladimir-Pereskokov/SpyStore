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
    class ShoppingCartRepo : RepoBase<ShoppingCartRecord>, IShopingCartRepo

    {
        private IProductRepo _productRepo;

        public ShoppingCartRepo(IProductRepo productRepo) : base() { _productRepo = productRepo; }
        public ShoppingCartRepo(DbContextOptions<StoreContext> options,
            IProductRepo productRepo) : base(options) { _productRepo = productRepo; }
        public override IEnumerable<ShoppingCartRecord> GetAll() =>
        table.OrderByDescending(c => c.DateTimeCreated);








        #region "IShoppingCartRepo"
        int IShopingCartRepo.Add(ShoppingCartRecord entity, int? quantityInStock, bool persist)
        {
            throw new System.NotImplementedException();
        }

        ShoppingCartRecord IShopingCartRepo.Find(int customerId, int productId)
        {
            throw new System.NotImplementedException();
        }

        CartRecordWithProductInfo IShopingCartRepo.GetShoppingCartRecord(int customerId, int productId)
        {
            throw new System.NotImplementedException();
        }

        int IShopingCartRepo.Purchase(int customerId)
        {
            throw new System.NotImplementedException();
        }

        int IShopingCartRepo.Update(ShoppingCartRecord entity, int? quantityInStock, bool persist)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
