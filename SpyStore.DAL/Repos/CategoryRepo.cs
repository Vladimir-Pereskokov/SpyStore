using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;


namespace SpyStore.DAL.Repos
{
    class CategoryRepo : RepoBase<Category>
    {
        public CategoryRepo() : 
            base() { }
        
        public CategoryRepo(DbContextOptions<StoreContext> options) : 
            base(options) { }

        public override IEnumerable<Category> GetAll() => table.OrderBy(x => x.CategoryName);
        public override IEnumerable<Category> GetRange(int skip, int take) => base.GetRange(table.OrderBy(x => x.CategoryName), skip, take);

    }
}

