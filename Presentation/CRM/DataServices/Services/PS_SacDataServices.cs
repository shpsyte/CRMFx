using CRM.DataServices;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using Data.Context;

namespace CRM.DataServices
{
    public class PS_SacDataServices : GDataServices, IDataServices<PS_Sac>
    {
        public PS_SacDataServices(b2yweb_entities context) : base(context)
        {

        }

        public void Add(PS_Sac entity)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return _context.PS_Sac.Count();
        }

        public int Count(Expression<Func<PS_Sac, bool>> where)
        {
            return _context.PS_Sac.Where(where).Count();
        }



        public void Delete(Expression<Func<PS_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(PS_Sac entity)
        {
            throw new NotImplementedException();
        }

        public PS_Sac Get(Expression<Func<PS_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PS_Sac> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PS_Sac> GetAll(Expression<Func<PS_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PS_Sac> Query()
        {
            return _context.PS_Sac.AsQueryable();
        }


        public PS_Sac GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PS_Sac> GetMany(Expression<Func<PS_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Update(PS_Sac entity)
        {
            throw new NotImplementedException();
        }

        public PS_Sac Find(params object[] key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PS_Sac> Query(Expression<Func<PS_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
    
}