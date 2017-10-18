using System.Collections.Generic;
using System.Text;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using SpyStore.Models.ViewModels.Base;

namespace SpyStore.DAL.Repos.Interfaces
{
    public interface IOrderDetailRepo : IRepo <OrderDetail>
    {
        IEnumerable<OrderDetailWithProductInfo> GetCustomersOrdersWithDetails(int customerId);
        IEnumerable<OrderDetailWithProductInfo> GetSingleOrderWithDetails(int orderId);

    }
}
