using CRM.App_Helpers;
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
    public class CampanhaOrcamentoController : BasePublicController
    {

        //
        // GET: /TipoLead/
        public ActionResult List()
        {
            var data = db.CampanhaOrcamento.ToList();
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
        public ActionResult Create(CampanhaOrcamento data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.campanhaorcamentoid = db.Database.SqlQuery<Int32>("select CampanhaOrcamentoSeq.NextVal from dual ").FirstOrDefault<Int32>();

            bool ja_existe = db.CampanhaOrcamento.Where(a => a.ano == data.ano).Count() > 0;
            if (ja_existe)
            {
                ModelState.AddModelError("ano", "Já existe um orçamento para este ano!");
                return View(data);
            }

            TryValidateModel(data);


            if (ModelState.IsValid)
            {
                db.CampanhaOrcamento.Add(data);
                try
                {
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Create") : RedirectToAction("List");
                }
                catch (DbEntityValidationException e)
                {
                    return View(data);
                }

            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id)
        {

            CampanhaOrcamento data = db.CampanhaOrcamento.Find(id);
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
        public ActionResult Edit(CampanhaOrcamento data, bool continueAdd, bool isDelete)
        {

            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.campanhaorcamentoid }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    CampanhaOrcamento dataDelete = db.CampanhaOrcamento.Find(data.campanhaorcamentoid);
                    db.CampanhaOrcamento.Remove(dataDelete);
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
                    return RedirectToAction("Edit", new { id = data.campanhaorcamentoid });
                }
                return RedirectToAction("List");
            }
        }









    }
}
