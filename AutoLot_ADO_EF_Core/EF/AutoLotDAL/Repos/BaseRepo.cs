using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoLotDAL.EF;
using AutoLotDAL.Models.Base;

namespace AutoLotDAL.Repos
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T: EntityBase, new()
    {
        private readonly DbSet<T> _table;
        private readonly AutoLotDbContext _ctx;

        public BaseRepo()
        {
            //context recreation is a costly operation, need to see in particular case how to share it
            _ctx = new AutoLotDbContext();
            _table = _ctx.Set<T>();
        }

        protected AutoLotDbContext Context => _ctx;

        public void Dispose()
        {
            _ctx?.Dispose();
        }

        internal int SaveChanges()
        {
            try
            {
                return _ctx.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //parallelism
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
            catch (CommitFailedException ex)
            {
                //transaction failed
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public T GetOne(int? id) => _table.Find(id);

        public virtual List<T> GetAll() => _table.ToList();

        public List<T> ExecuteQuery(string sql) => _table.SqlQuery(sql).ToList();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects) => _table.SqlQuery(sql, sqlParametersObjects).ToList();

        public int Add(T entity)
        {
            _table.Add(entity);
            return SaveChanges();
        }

        public int AddRange(IList<T> entities)
        {
            _table.AddRange(entities);
            return SaveChanges();
        }

        public int Save(T entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public int Delete(int id, byte[] timeStamp)
        {
            _ctx.Entry(new T {Id = id, Timestamp = timeStamp}).State = EntityState.Deleted;
            return SaveChanges();
        }

        public int Delete(T entity)
        {
            _ctx.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }
    }
}
