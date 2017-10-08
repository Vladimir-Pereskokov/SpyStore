using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SpyStore.DAL.EF;
using System.Linq;
using SpyStore.Models.Entities;
using SpyStore.Models.Entities.Base;

namespace SpyStore.DAL.Repos.Base
{
    public abstract class RepoBase<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {

        protected StoreContext db = null;
        protected DbSet<T> table = null;

        protected RepoBase()
        {
            db = new StoreContext();
            table = db.Set<T>();
        }

        protected RepoBase(DbContextOptions<StoreContext> options)
        {
            db = new StoreContext(options);
            table = db.Set<T>();
        }

        public virtual StoreContext Context() => db;



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                    table = null;

                    if (!(db == null))
                    {
                        db.Dispose();
                        db = null;
                    }

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RepoBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public virtual int Count => table.Count();

        public virtual bool HasChanges => db.ChangeTracker.HasChanges();

        public virtual int Add(T value, bool persist = true)
        {
            table.Add(value);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> values, bool persist = true)
        {
            table.AddRange(values);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(int id, byte[] timeStamp, bool persist = true)
        {
            T entity = GetEntityFromChangeTracker(id);
            if (!(entity == null))
            {
                if (entity.TimeStamp == timeStamp)
                {
                    return Delete(entity, persist);
                }
                else
                { throw new Exception("Unable to delete due to concurrency violation"); }
            }
            else
            {
                entity = new T { Id = id, TimeStamp = timeStamp };
                db.Entry<T>(entity).State = EntityState.Deleted;
                return persist ? db.SaveChanges() : 0;
            }

        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }



        internal T GetEntityFromChangeTracker(int? id)
        {
            return db.ChangeTracker.Entries<T>()
                .Select((EntityEntry e) => (T)e.Entity)
                .FirstOrDefault(x => x.Id == id);
        }




        public virtual T Find(int? id)
        {
            if (id.HasValue)
            {
                T entity = GetEntityFromChangeTracker(id);
                if (entity == null && id > 0)
                {
                    entity = table.Find(id);
                }
                return entity;
            }
            return null;
        }

        public virtual IEnumerable<T> GetAll() => table;

        public virtual T GetFirst() => table.FirstOrDefault();

        public virtual IEnumerable<T> GetRange(int skip, int take)
        {
            return GetRange(table, skip, take);
        }

        internal IEnumerable<T> GetRange(IQueryable<T> qry, int skip, int take)
            => qry.Skip(skip).Take(take);


        public virtual int SaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException eDB)
            {
                Console.WriteLine(eDB);
                throw;
            }
            catch (RetryLimitExceededException rEx)
            {
                Console.WriteLine(rEx);
                throw rEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int Update(T entity, bool persist = true)
        {
            table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

    }
}
