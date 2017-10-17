using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.Entities;

namespace SpyStore.Models.ViewModels
{
    class CartRecordWithProductInfo : Base.ProductAndCategoryBase
    {
        public CartRecordWithProductInfo() : base() { }

        #region "Properties"

        [Display(Name = "Date Created"), DataType(DataType.Date)]
        public DateTime? DateCreated { get; set; }

        public int? CustomerId { get; set; }

        private int _Qty;
        public int Quantity
        { get
            {               
               return _Qty;
            }

            set
            {
                _Qty = value;
            }
        }

        [Display(Name = "Line Total"), DataType(DataType.Currency) ]
        public decimal? LineItemTotal { get; set; }

        #endregion

    }
}
