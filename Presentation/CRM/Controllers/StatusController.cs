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
    public class StatusController : BasePublicController
    {

        //
        // GET: /TipoLead/
        public ActionResult List()
        {
            var data = db.Status.ToList();
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
        public ActionResult Create(Status data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.statusId = db.Database.SqlQuery<Int32>("select STATUSSEQ.NextVal from dual ").FirstOrDefault<Int32>();

            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.Status.Add(data);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create") : RedirectToAction("List");
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id)
        {
            Status data = db.Status.Find(id);
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
        public ActionResult Edit(Status data, bool continueAdd, bool isDelete)
        {
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.statusId }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    Status dataDelete = db.Status.Find(data.statusId);
                    db.Status.Remove(dataDelete);
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
                    return RedirectToAction("Edit", new { id = data.statusId });
                }
                return RedirectToAction("List");
            }
        }







    }
}
