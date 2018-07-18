using Domain.Entity;
using Services.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CRM.Controllers
{
    [NoCache]
    public class TipoSacController : BasePublicController
    {
         //
        // GET: /TipoLead/
        public ActionResult List()
        {
            var data = db.PS_Tipo_Sac.ToList();
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
        public ActionResult Create(PS_Tipo_Sac data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.cod_tipo = db.Database.SqlQuery<Int32>("select PS_Tipo_Sac_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.PS_Tipo_Sac.Add(data);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create") : RedirectToAction("List");
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id)
        {
            PS_Tipo_Sac data = db.PS_Tipo_Sac.Find(id);
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
        public ActionResult Edit(PS_Tipo_Sac data, bool continueAdd, bool isDelete)
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
                    PS_Tipo_Sac dataDelete = db.PS_Tipo_Sac.Find(data.cod_tipo);
                    db.PS_Tipo_Sac.Remove(dataDelete);
                    db.SaveChanges();
                    RedirectToAction("List");
                }
                catch (DbEntityValidationException e)
                {
                    
                    //foreach (var result in e.EntityValidationErrors)
                    // {
                    //   foreach (var error in result.ValidationErrors)
                    // {
                    ModelState.AddModelError("", e.Message);
                    //}
                    // }
                    return RedirectToAction("Edit", new { id = data.cod_tipo });
                }
                return RedirectToAction("List");
            }
        }
    }
}
