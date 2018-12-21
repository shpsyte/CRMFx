using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [AllowAnonymous]
    public class DashboardController : Controller
    {
        private b2yweb_entities db = null;

        [AllowAnonymous]
        [HttpGet]
        public ActionResult index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetDashboard()
        {
            string dta_ini = "01/10/2018";
            string dta_fim = "31/12/2018";

            db = new b2yweb_entities("oracle");

            db.Database.ExecuteSqlCommand(string.Format("Begin spcGetDashboard(\'{0}\',\'{1}\'); end;", dta_ini, dta_fim));

            var Data = db.Dashboard.ToList();
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

    }
}
