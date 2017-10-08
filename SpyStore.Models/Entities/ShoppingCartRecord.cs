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
    [Table("ShoppingCartRecords", Schema = "Store")]
    public class ShoppingCartRecord : EntityBase
    {
        public ShoppingCartRecord() : base() { }

        [Required]
        public int ProductID { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateTimeCreated { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [ForeignKey(nameof(CustomerID))]
        public Customer Customer { get; set; }

        public int Quantity { get; set; }

        [NotMapped()]
        public decimal LineItemTotal { get; set; }


    }
}

