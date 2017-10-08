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
    [Table("Customers", Schema = "Store")]
    public class Customer : EntityBase
    {
        public Customer() : base() { }
        ~Customer() { }
        [MaxLength(50), DataType(DataType.Text), Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required, EmailAddress, DataType(DataType.Text), MaxLength(50), Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required, DataType(DataType.Password), MaxLength(50), Display(Name = "Password")]
        public string Password { get; set; }

        [InverseProperty(nameof(Order.Customer))]
        public List<Order> Orders { get; set; } = new List<Order>();

        [InverseProperty(nameof(ShoppingCartRecord.Customer))]
        public List<ShoppingCartRecord> ShoppingCartRecords { get; set; } = new List<ShoppingCartRecord>();

    }
}

