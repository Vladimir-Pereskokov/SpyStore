using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.Entities;




namespace SpyStore.Models.ViewModels
{
   public class OrderDetailWithProductInfo :Base.ProductAndCategoryBase 
    {
        public OrderDetailWithProductInfo() : base() { }

        #region "Properties"
        public int OrderId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [DataType(DataType.Currency), Display (Name = "Total")]
        public decimal? LineItemTotal { get; set; }
        #endregion
    }
}
