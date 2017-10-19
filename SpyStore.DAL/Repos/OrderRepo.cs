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
    public class OrderRepo : RepoBase<Order>, IOrderRepo
    {
        private IOrderDetailRepo _detailRepo;

        public OrderRepo(IOrderDetailRepo detailRepo) : base() { _detailRepo = detailRepo; }
        public OrderRepo(DbContextOptions<StoreContext> options,
            IOrderDetailRepo detailRepo) : base(options) { _detailRepo = detailRepo; }

        ~OrderRepo() { _detailRepo = null; }

        public override IEnumerable<Order> GetAll() =>
            table.OrderByDescending(o => o.OrderDate);

        public override IEnumerable<Order> GetRange(int skip, int take) =>
            base.GetRange(table.OrderByDescending(o => o.OrderDate), skip, take);

        #region "IOrderRepo implementation"
        OrderWithDetailsAndProductInfo IOrderRepo.GetOneWithDetails(int customerId, int orderId) =>
            table
            .Where(o => o.CustomerID == customerId && o.Id == orderId)
            .Include(o => o.Details)
            .Select(o => new OrderWithDetailsAndProductInfo
            {
                Id = orderId,
                CustomerId = customerId,
                DateOrdered = o.OrderDate ,
                OrderTotal = o.OrderTotal,
                DateShipped = o.ShipDate,
                OrderDetails = _detailRepo.GetSingleOrderWithDetails(orderId).ToList()
            }
            )
            .FirstOrDefault();


         IEnumerable<Order> IOrderRepo.GetOrderHistory(int customerId) 
            //need .Select to remove navigation properties
            =>
            table
            .Where(o => o.CustomerID == customerId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o=> new Order {
                Id = o.Id,
                TimeStamp = o.TimeStamp,
                CustomerID = o.CustomerID,
                OrderDate = o.OrderDate,
                OrderTotal = o.OrderTotal,
                ShipDate = o.ShipDate
            });
        #endregion
    }
}
