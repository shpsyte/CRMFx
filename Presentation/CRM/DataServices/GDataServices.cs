using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.DataServices
{
    public class GDataServices : IDisposable
    {
        public b2yweb_entities _context;

        public GDataServices(b2yweb_entities context)
        {
            this._context = context;

        }
        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}