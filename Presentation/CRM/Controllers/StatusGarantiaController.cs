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
    [Services.Functions.NoCache]
    public class StatusGarantiaController : BasePublicController
    {

        //
        // GET: /TipoLead/
        public ActionResult List()
        {
            var data = db.GE_Status_Garantia.ToList();
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
        public ActionResult Create(GE_Status_Garantia data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.Cod_Status = db.Database.SqlQuery<Int32>("select GE_Status_Garantia_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            if (string.IsNullOrEmpty(data.ind_finalizacao))
            {
                data.ind_finalizacao = "N";
            }
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.GE_Status_Garantia.Add(data);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create") : RedirectToAction("List");
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id)
        {
            GE_Status_Garantia data = db.GE_Status_Garantia.Find(id);
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
        public ActionResult Edit(GE_Status_Garantia data, bool continueAdd, bool isDelete)
        {
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.Cod_Status }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    GE_Status_Garantia dataDelete = db.GE_Status_Garantia.Find(data.Cod_Status);
                    db.GE_Status_Garantia.Remove(dataDelete);
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
                    return RedirectToAction("Edit", new { id = data.Cod_Status });
                }
                return RedirectToAction("List");
            }
        }






    }
}
