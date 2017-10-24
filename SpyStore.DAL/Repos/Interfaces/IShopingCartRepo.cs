using System.Collections.Generic;
using System.Text;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using SpyStore.Models.ViewModels.Base;

namespace SpyStore.DAL.Repos.Interfaces
{
    public interface IShoppingCartRepo:IRepo<ShoppingCartRecord>
    {
        CartRecordWithProductInfo GetShoppingCartRecord(int customerId, int productId);
        int Purchase(int customerId);
        ShoppingCartRecord Find(int customerId, int productId);
        int Update(ShoppingCartRecord entity, int? quantityInStock, bool persist);
        int Add(ShoppingCartRecord entity, int? quantityInStock, bool persist);
    }
}
