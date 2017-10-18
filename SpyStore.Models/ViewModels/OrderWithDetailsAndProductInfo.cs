using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.Entities;

namespace SpyStore.Models.ViewModels
{
    public class OrderWithDetailsAndProductInfo : EntityBase
    {
        public OrderWithDetailsAndProductInfo() : base() { }

        #region "Properties"
        public int CustomerId { get; set; }

        [Display(Name = "Order Total"), DataType(DataType.Currency)]
        public decimal? OrderTotal { get; set; }

        [Display(Name = "Date Ordered"), DataType(DataType.Date)]
        public DateTime DateOrdered { get; set; }

        [Display(Name = "Date Shipped"), DataType(DataType.Date)]
        public DateTime? DateShipped { get; set; }
        public IList<OrderDetailWithProductInfo> OrderDetails {get;set;}
        #endregion
    }
}
