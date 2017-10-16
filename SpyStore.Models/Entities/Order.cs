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
    [Table("Orders", Schema = "Store")]
    public class Order : EntityBase
    {
        public Order() { }
        ~Order() { }
        [InverseProperty(nameof(OrderDetail.Order))]
        public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        [Required]
        public int CustomerID { get; set; }

        [ForeignKey(nameof(CustomerID))]
        public Customer Customer { get; set; }

        [DataType(DataType.Date), Display(Name = "Date ordered")]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date), Display(Name = "Date shipped")]
        public DateTime ShipDate { get; set; }

        [Display(Name = "Order Total")]
        public decimal? OrderTotal {get;set;}
    }
}
