using System;
using System.Collections.Generic;
using System.Text;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using SpyStore.Models.ViewModels.Base;


namespace SpyStore.DAL.Repos.Interfaces
{
    public interface IProductRepo :IRepo<Product>
    {
        IEnumerable<ProductAndCategoryBase> Search(string SearchString);
        IEnumerable<ProductAndCategoryBase> GetAllWithCategoryName();
        IEnumerable<ProductAndCategoryBase> GetProductsForCategory(int categoryId);
        IEnumerable<ProductAndCategoryBase> GetFeaturedWithCategoryName();
        ProductAndCategoryBase GetOneWithCategoryName(int id);
    }
}
