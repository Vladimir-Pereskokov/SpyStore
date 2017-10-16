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
    [Table("Products", Schema = "Store")]
    public class Product : EntityBase
    {
        public Product() : base() { }

        [MaxLength(3800)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string ModelName { get; set; }

        public bool IsFeatured { get; set; }
        [MaxLength(50)]
        public string ModelNumber { get; set; }

        [MaxLength(150)]
        public string ProductImage { get; set; }

        [MaxLength(150)]
        public string ProductImageThumb { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }

        [InverseProperty(nameof(ShoppingCartRecord.Product))]
        public List<ShoppingCartRecord> ShoppingCartRecords { get; set; } = new List<ShoppingCartRecord>();

        [InverseProperty(nameof(Entities.OrderDetail.Product))]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}

