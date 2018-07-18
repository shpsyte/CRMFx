using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using Data.Context;
using System.Web.Mvc;
namespace CRM.DataServices
{
    public class PS_Situacao_SacDataServices : GDataServices, IDataServices<PS_Situacao_Sac>
    {
        public class SituacaoType
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }
        /// <summary>
        /// Retorna lista fixa de tipos
        /// </summary>
        /// <returns>Retora Lista</returns>
        public IList<SituacaoType> GetTipos()
        {
            List<SituacaoType> tipos = new List<SituacaoType>
                {
                    new SituacaoType()  { Id = 1, Nome = "Abertos"  },
                    new SituacaoType()  { Id = 2, Nome = "Fechados"  },
                    new SituacaoType()  { Id = 3, Nome = "Aguardando SAC/GARANTIA"  },
                    new SituacaoType()  { Id = 4, Nome = "Todos"  }
                };
            return tipos;
        }
        public PS_Situacao_SacDataServices(b2yweb_entities context) : base(context)
        {
            //this._context = context;
        }
        public void Add(PS_Situacao_Sac entity)
        {
            throw new NotImplementedException();
        }
        
        public int Count()
        {
            throw new NotImplementedException();
        }
        public int Count(Expression<Func<PS_Situacao_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }
        public void Delete(Expression<Func<PS_Situacao_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }
        public void Delete(PS_Situacao_Sac entity)
        {
            throw new NotImplementedException();
        }
        public PS_Situacao_Sac Get(Expression<Func<PS_Situacao_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<PS_Situacao_Sac> GetAll()
        {
            return this._context.PS_Situacao_Sac.AsNoTracking().ToList();
        }
        public PS_Situacao_Sac GetById(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<PS_Situacao_Sac> GetMany(Expression<Func<PS_Situacao_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }
        public IQueryable<PS_Situacao_Sac> Query()
        {
            throw new NotImplementedException();
        }
        public void Update(PS_Situacao_Sac entity)
        {
            throw new NotImplementedException();
        }

        public PS_Situacao_Sac Find(params object[] key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PS_Situacao_Sac> GetAll(Expression<Func<PS_Situacao_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PS_Situacao_Sac> Query(Expression<Func<PS_Situacao_Sac, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}