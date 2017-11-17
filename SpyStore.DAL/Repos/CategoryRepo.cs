using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using SpyStore.DAL.Repos.Interfaces;


namespace SpyStore.DAL.Repos
{
    public class CategoryRepo : RepoBase<Category> , ICategoryRepo
    {
        public CategoryRepo() : 
            base() { }
        
        public CategoryRepo(DbContextOptions<StoreContext> options) : 
            base(options) { }

        public override IEnumerable<Category> GetAll() => table.OrderBy(x => x.CategoryName);

      

        public override IEnumerable<Category> GetRange(int skip, int take) => base.GetRange(table.OrderBy(x => x.CategoryName), skip, take);


        #region "ICategoryRepo"

        public IEnumerable<Category> GetAllWithProdicts() => table.Include(x => x.Products);


        public Category GetOneWithProducts(int? id) => table.Include(x => x.Products).SingleOrDefault(x => x.Id == id);
        

        #endregion

    }
}

