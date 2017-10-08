﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpyStore.Models.Entities.Base;

namespace SpyStore.Models.Entities
{
    [Table("Categories", Schema = "Store")]
    public class Category : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50), Display(Name = "Category")] public string CategoryName { get; set; }

        [InverseProperty(nameof(Product.Category))]
        public List<Product> Products { get; set; } = new List<Product>();


    }
}