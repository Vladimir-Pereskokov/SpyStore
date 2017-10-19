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
    public class OrderDetailRepo : RepoBase<OrderDetail>, IOrderDetailRepo
    {

        public OrderDetailRepo() : base() { }
        public OrderDetailRepo(DbContextOptions<StoreContext> options) : base(options) { }


        public override IEnumerable<OrderDetail> GetAll() =>
            table.OrderBy(d => d.Id);
        public override IEnumerable<OrderDetail> GetRange(int skip, int take) =>
            base.GetRange(table.OrderBy(d => d.Id), skip, take);

        private IEnumerable<OrderDetailWithProductInfo> GetRecords(IQueryable<OrderDetail> qry)
            => qry
            .OrderBy(d => d.Product.ModelName)
            .Include(d => d.Product)
            .Select(d=> new OrderDetailWithProductInfo
            {
                OrderId = d.OrderID,
                ProductId = d.ProductID,
                Quantity = d.Quantity,
                UnitCost = d.UnitCost,
                UnitPrice = d.Product.UnitPrice,
                LineItemTotal = d.LineItemTotal,
                Description = d.Product.Description,
                ModelName = d.Product.ModelName,
                ProductImage = d.Product.ProductImage,
                ProductImageLarge = d.Product.ProductImageLarge,
                ProductImageThumb = d.Product.ProductImageThumb,
                ModelNumber = d.Product.ModelNumber,
                CategoryName = d.Product.Category.CategoryName,
                CategoryId = d.Product.CategoryID 
            }
              );




        #region "IOrderDetailRepo implementation"

        public IEnumerable<OrderDetailWithProductInfo> GetCustomersOrdersWithDetails(int customerId)
            => GetRecords(table.Where(d => d.Order.CustomerID == customerId));


        public IEnumerable<OrderDetailWithProductInfo> GetSingleOrderWithDetails(int orderId)
            => GetRecords(table.Where(d => d.OrderID == orderId));
        #endregion

    }
}
