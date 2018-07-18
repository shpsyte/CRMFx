using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class TipoLeadController : BasePublicController
    {
        //
        // GET: /TipoLead/

        public ActionResult List()
        {
           var data = db.Ps_Tipo_Lead.ToList();
            return View(data);
        }

        //
        // GET: /TipoLead/Details/5

        //
        // GET: /TipoLead/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoLead/Create

       
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(Ps_Tipo_Lead data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.cod_tipo = db.Database.SqlQuery<Int32>("select Ps_Tipo_Lead_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            TryValidateModel(data);

            if (ModelState.IsValid)
            {
                db.Ps_Tipo_Lead.Add(data);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create") : RedirectToAction("List");
            }

            return View(data);
        }

        //
        // GET: /TipoLead/Edit/5

        public ActionResult Edit(int id)
        {
            Ps_Tipo_Lead data = db.Ps_Tipo_Lead.Find(id);

            if (data == null)
            {
                return InvokeHttpNotFound();
            }


            return View(data);
        }

        //
        // POST: /TipoLead/Edit/5

        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(Ps_Tipo_Lead data, bool continueAdd, bool isDelete)
        {
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.cod_tipo }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    Ps_Tipo_Lead dataDelete = db.Ps_Tipo_Lead.Find(data.cod_tipo);
                    db.Ps_Tipo_Lead.Remove(dataDelete);
                    db.SaveChanges();
                    RedirectToAction("List");
                }
                catch (DbEntityValidationException e)
                {
                    //foreach (var result in e.EntityValidationErrors)
                    // {
                    //   foreach (var error in result.ValidationErrors)
                    // {
                    ModelState.AddModelError("",e.Message);
                    //}
                    // }
                    return RedirectToAction("Edit", new { id = data.cod_tipo });
                }

                return RedirectToAction("List");
            }
        }

      
    }
}
