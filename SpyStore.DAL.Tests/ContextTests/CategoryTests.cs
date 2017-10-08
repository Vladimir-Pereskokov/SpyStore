using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using System.Linq;
using SpyStore.Models;
using Xunit;
using SpyStore.Models.Entities;


namespace SpyStore.DAL.Tests.ContextTests
{
    [Collection("SpyStore.DAL")]
    public class CategoryTests : IDisposable
    {
        private StoreContext _db = null;

        public CategoryTests()
        {
            _db = new StoreContext();
            CleanDatabase();
        }

        public void Dispose()
        {
            if (!(_db == null))
            {
                CleanDatabase();
                _db.Dispose();
                _db = null;
            }
        }
        private void CleanDatabase()
        {
            if (!(_db == null))
            {


                _db.Database.ExecuteSqlCommand(@"Delete from Store.Categories");
                _db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"Store.Categories\", RESEED, -1);");


            }

        }
        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }


        [Fact]
        public void ShouldAddACategoryWithDbSet()
        {
            var cat = new Category() { CategoryName = "Foo" };
            _db.Categories.Add(cat);
            Assert.Equal(EntityState.Added, _db.Entry(cat).State);
            Assert.True(cat.Id < 0);
            Assert.Null(cat.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(cat).State);
            Assert.Equal(0, cat.Id);
            Assert.NotNull(cat.TimeStamp);

        }


        [Fact]
        public void ShouldAddACategoryWithContext()
        {
            var cat = new Category() { CategoryName = "Foo" };
            _db.Add(cat);
            Assert.Equal(EntityState.Added, _db.Entry(cat).State);
            Assert.True(cat.Id < 0);
            Assert.Null(cat.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(cat).State);
            Assert.True(cat.Id > -1);
            Assert.NotNull(cat.TimeStamp);
            Assert.Equal(1, _db.Categories.Count());
        }

        [Fact]
        public void ShouldGetAllCetegoriesOrderedByName()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            _db.SaveChanges();
            category.CategoryName = "Bar";
            _db.Categories.Update(category);
            Assert.Equal(EntityState.Modified,
                _db.Entry(category).State);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);
            StoreContext context;
            using (context = new StoreContext())
            {
                Assert.Equal("Bar", context.Categories.First().CategoryName);
            }
        }

        [Fact]
        public void ShouldThrowUpdateEx()
        {
            var cat = new Category() { CategoryName = "Foo" };
            _db.Categories.Add(cat);
            cat.CategoryName = "Bar";
            Assert.Throws<InvalidOperationException>(() => _db.Categories.Update(cat));


        }

        [Fact]
        public void ShouldDeleteACategory()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            _db.SaveChanges();
            Assert.Equal(1, _db.Categories.Count());
            _db.Categories.Remove(category);
            Assert.Equal(EntityState.Deleted, _db.Entry(category).State);
            _db.SaveChanges();
            Assert.Equal(EntityState.Detached, _db.Entry(category).State);
            Assert.Equal(0, _db.Categories.Count());

        }


        [Fact]
        public void ShouldDeleteCategoryWithStampData()
        {
            var cat = new Category { CategoryName = "Foo" };
            _db.Categories.Add(cat);
            _db.SaveChanges();
            using (var context = new StoreContext())
            {
                Category catToDelete = new Category { Id = cat.Id, TimeStamp = cat.TimeStamp };
                context.Entry(catToDelete).State = EntityState.Deleted;
                var affected = context.SaveChanges();
                Assert.Equal(1, affected);
            }
        }


        [Fact]
        public void ShouldNotDeleteCategoryWithoutStampData()
        {

            var cat = new Category { CategoryName = "Foo" };
            _db.Categories.Add(cat);
            _db.SaveChanges();
            var context = new StoreContext();
            var catToDelete = new Category { Id = cat.Id };

            //context.Entry<Category>(catToDelete).CurrentValues.
            try
            {
                context.Entry<Category>(catToDelete).State = EntityState.Unchanged;
                //context.Categories.Attach(catToDelete);
                context.Categories.Remove(catToDelete);
            }
            catch (Exception e)
            {
                throw e;
            }

            var ex = Assert.Throws<DbUpdateConcurrencyException>(() =>
                   context.SaveChanges());
            Assert.Equal(1, ex.Entries.Count);
            Assert.Equal(cat.Id, ((Category)ex.Entries[0].Entity).Id);



        }


    }

}
