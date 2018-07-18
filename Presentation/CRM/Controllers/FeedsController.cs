using CRM.Models;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [CustomAuthorize(AccessLevel = "Admin;User" )]
    public class FeedsController : BasePublicController
    {
        //
        // GET: /Feeds/ 

        public ActionResult Index()
        {
            
            int Ano = System.DateTime.Now.Year;
            int Mes = System.DateTime.Now.AddMonths(-3).Month;

            var Itens  = db.ListaComentarios.Where(a => a.tipo_nota != "NOTE" && a.dta_inclusao.Year == Ano /*&& a.dta_inclusao.Month >= Mes*/).OrderByDescending(a => a.dta_inclusao).ToList();
            var itens2 = db.ListaComentarios.Where(a => a.tipo_nota == "NOTE" && a.dta_inclusao.Year == Ano /*&& a.dta_inclusao.Month >= Mes */).ToList();

            ViewData["ListaComentarios"] = itens2;
            return View(Itens);
        }

    }
}
