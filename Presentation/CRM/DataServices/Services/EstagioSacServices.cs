using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Context;

namespace CRM.DataServices
{
    public class EstagioSacServices : DataServices<PS_Estagio_Sac>, IEstagioSacServices
    {
        private readonly b2yweb_entities _context;
        public EstagioSacServices(b2yweb_entities context) : base(context)
        {
            this._context = context;
        }


    }
}