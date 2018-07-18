using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Data.Context;
using System.Data.Entity;
using System.Data;

namespace CRM.DataServices
{
    public class DataServices<T> : GDataServices, IDataServices<T> where T : class
    {
        private DbSet<T> _entity;
        public DataServices(b2yweb_entities context) : base(context)
        {
            this._context = context;
            _entity = context.Set<T>();
        }

        public void Add(T entity)
        {
            this._entity.Add(entity);
            this._context.SaveChanges();
        }

        public int Count()
        {
            return this._entity.Count();
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return this._entity.Count(where);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            this._entity.Where(where).ToList().ForEach(del => _context.Set<T>().Remove(del));
            this._context.SaveChanges();
        }

        public void Delete(T entity)
        {
            this._entity.Remove(entity);
            this._context.SaveChanges();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return this._entity.Where(where).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return this._entity.ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> where)
        {
            return this._entity.Where(where).ToList();
        }

        public T Find(params object[] key)
        {
            return this._entity.Find(key);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return this._entity.Where(where).ToList();
        }

        public IQueryable<T> Query()
        {
            return this._entity.AsNoTracking().AsQueryable();
        }


        public IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return this._entity.Where(where).AsNoTracking().AsQueryable();
        }

        public void Update(T entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            this._context.SaveChanges();

        }
    }
}