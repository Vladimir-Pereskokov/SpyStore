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
    public class CustomerRepo:RepoBase<Customer>, ICustomerRepo
    {
        public CustomerRepo() : base() { }
        public CustomerRepo(DbContextOptions<StoreContext> options) : base(options) { }
        public override IEnumerable<Customer> GetAll() 
            => table.OrderBy(c => c.FullName);
        public override IEnumerable<Customer> GetRange(int skip, int take) 
            => base.GetRange(table.OrderBy(c => c.FullName), skip, take);        
    }
}
