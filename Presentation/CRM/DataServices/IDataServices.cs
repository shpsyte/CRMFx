using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataServices
{
        
        public interface IDataServices<T> where T : class
        {
        //void Adiciona(class t where>
        // Marks an entity as new
            void Add(T entity);
             // Marks an entity as modified
            void Update(T entity);
            // Marks an entity to be removed
            void Delete(T entity);
            void Delete(Expression<Func<T, bool>> where);
            // Get an entity by int id
            T Find(params object[] key);
            // Get an entity using delegate
            T Get(Expression<Func<T, bool>> where);
            // Gets all entities of type T
            IEnumerable<T> GetAll();
            IEnumerable<T> GetAll(Expression<Func<T, bool>> where);
            // Gets entities using delegate
            IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

            IQueryable<T> Query();
        IQueryable<T> Query(Expression<Func<T, bool>> where);


        int Count();
            int Count(Expression<Func<T, bool>> where);
        }
    }
