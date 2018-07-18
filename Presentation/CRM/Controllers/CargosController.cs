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
    public class CargosController : BasePublicController
    {
        //
        // GET: /Cargos/
        public ActionResult List()
        {
            var cargos = db.Cargos.ToList();
            return View(cargos);
        }



        //
        // GET: /Cargos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cargos/Create

        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(Cargos cargos, bool continueAdd, FormCollection form)
        {

            ModelState.Clear();
            cargos.cod_cargo = db.Database.SqlQuery<Int32>("select Ps_Cargos_Crm_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            TryValidateModel(cargos);

            if (ModelState.IsValid)
            {
                db.Cargos.Add(cargos);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create") : RedirectToAction("List");
            }

            return View(cargos);
        }

        //
        // GET: /Cargos/Edit/5
        public ActionResult Edit(int id)
        {
            Cargos cargos = db.Cargos.Find(id);

            if (cargos == null)
            {
                return InvokeHttpNotFound();
            }


            return View(cargos);
        }

        //
        // POST: /Cargos/Edit/5

        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(Cargos cargos, bool continueAdd, bool isDelete)
        {
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(cargos).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = cargos.cod_cargo }) : RedirectToAction("List");
                }
                return View(cargos);
            }
            else
            {
                try
                {
                    Cargos cargoDelete = db.Cargos.Find(cargos.cod_cargo);
                    db.Cargos.Remove(cargoDelete);
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
                    return RedirectToAction("Edit", new { id = cargos.cod_cargo });
                }

                return RedirectToAction("List");
            }
            
        }

        //
        // GET: /Cargos/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Cargos/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
