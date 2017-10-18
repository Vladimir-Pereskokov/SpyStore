using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.Entities;



namespace SpyStore.Models.ViewModels.Base
{
    public class ProductAndCategoryBase : EntityBase
    {
        public ProductAndCategoryBase() : base() { }

        #region "Properties"
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public int ProductId { get; set; }

        [MaxLength(3800)]
        public string Description { get; set; }

        [MaxLength(50), 
            Display(Name = "Model")]
        public string ModelName{get; set;}


        [Display(Name = "Is Featured Product")]
        public bool IsFeatured { get; set; }

        [MaxLength(50), Display(Name = "Model Number")]
        public string ModelNumber { get; set; }

        [MaxLength(150)]
        public string ProductImage { get; set; }

        [MaxLength(150)]
        public string ProductImageLarge { get; set; }

        [MaxLength(150)]
        public string ProductImageThumb { get; set; }


        [Display(Name = "Unit Cost"), DataType(DataType.Currency)]
        public decimal UnitCost { get; set; }


        [DataType(DataType.Currency), Display(Name = "Current Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }

        #endregion

        
    }
}
