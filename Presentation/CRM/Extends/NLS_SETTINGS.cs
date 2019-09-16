using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Extends
{
    public static class NLS_SETTINGS
    {


        public static void alter_session_nl(b2yweb_entities db)
        {
            
            string sql_alter = @" ALTER SESSION SET nls_date_format = 'dd/mm/yyyy hh24:mi:ss'  ";

            db.Database.ExecuteSqlCommand(sql_alter);
        }
    }
}