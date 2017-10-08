using System;
using System.Collections.Generic;
using System.Text;
using SpyStore.Models.Entities.Base;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyStore.Models.Entities
{
    [Table("OrderDetails", Schema = "Store")]
    public class OrderDetail : EntityBase
    {
        public OrderDetail() : base() { }

        ~OrderDetail() { }

        [Required]
        public int ProductID { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }

        [Required]
        public int OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order Order { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required, DataType(DataType.Currency), Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Total")]
        public decimal? LineItemTotal { get; set; }

    }
}
