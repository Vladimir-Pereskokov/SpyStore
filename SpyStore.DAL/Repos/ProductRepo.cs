using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.ViewModels.Base;

namespace SpyStore.DAL.Repos
{
    public class ProductRepo : RepoBase<Product>, IProductRepo
            {

        public ProductRepo(DbContextOptions<StoreContext> options) : base(options)
        {
            table = base.Context().Products;
        }

        public ProductRepo() : base()
        {
            table = base.Context().Products;
        }


        public override IEnumerable<Product> GetAll() => table.OrderBy(x => x.ModelName);

        internal ProductAndCategoryBase GetRecord(Product p, Category c) => new ProductAndCategoryBase()
        {
            CategoryName = c.CategoryName,
            CategoryId = p.CategoryID,
            UnitPrice = p.UnitPrice ,
            Description = p.Description,
            IsFeatured = p.IsFeatured,
            Id = p.Id,
            ModelName = p.ModelName,
            ModelNumber = p.ModelNumber,
            ProductImage = p.ProductImage,
            ProductImageLarge = p.ProductImageLarge,
            ProductImageThumb = p.ProductImageThumb,
            TimeStamp = p.TimeStamp,
            UnitCost = p.UnitCost,
            UnitsInStock = p.UnitsInStock
        };

        

        #region "IProductRepo"

        public IEnumerable<ProductAndCategoryBase> GetAllWithCategoryName()
            => table.Include(p => p.Category)
            .Select(item => GetRecord(item, item.Category))
            .OrderBy(x => x.ModelName);


        public IEnumerable<ProductAndCategoryBase> GetFeaturedWithCategoryName()
            => table.Where(p => p.IsFeatured)
            .Include(p => p.Category)
            .Select(item => GetRecord(item, item.Category));

        public IEnumerable<ProductAndCategoryBase> GetProductsForCategory(int categoryId) 
            => table.Where(p => p.CategoryID == categoryId)
                .Include(p => p.Category)
                .Select(item => GetRecord(item, item.Category))
                .OrderBy(x => x.ModelName);



        public ProductAndCategoryBase GetOneWithCategoryName(int id)
=> table 
            .Where (p => p.Id == id)
            .Include(p => p.Category)
            .Select(item => GetRecord(item, item.Category))
            .SingleOrDefault();

        public IEnumerable<ProductAndCategoryBase> Search(string SearchString)
            => table
            .Where(p => p.Description.ToLower().Contains(SearchString) || p.ModelName.ToLower().Contains(SearchString))
            .Include(p=>p.Category)
            .Select(item => GetRecord(item , item.Category))
            .OrderBy(r => r.ModelName)
            
            ;
#endregion 

    }
}
