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
    public class EstagioController : BasePublicController
    {

        //
        // GET: /TipoLead/
        public ActionResult List()
        {
            var data = db.Estagio.ToList().OrderBy(a => a.estagioid_anterior);
            return View(data);
        }

        //
        // GET: /TipoLead/Details/5
        //
        // GET: /TipoLead/Create
        public ActionResult Create()
        {
            ViewBag.statusid = new SelectList(db.Status, "statusId", "descricao");
            ViewBag.estagioid_proximo = new SelectList(db.Estagio, "estagioId", "descricao");
            ViewBag.estagioid_anterior = new SelectList(db.Estagio, "estagioId", "descricao");
            return View();
        }
        //
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(Estagio data, bool continueAdd, FormCollection form)
        {
            ViewBag.statusid = new SelectList(db.Status, "statusId", "descricao", data.statusid);
            ViewBag.estagioid_proximo = new SelectList(db.Estagio, "estagioId", "descricao", data.estagioid_proximo);
            ViewBag.estagioid_anterior = new SelectList(db.Estagio, "estagioId", "descricao", data.estagioid_anterior);

            ModelState.Clear();
            data.estagioId = db.Database.SqlQuery<Int32>("select ESTAGIOSEQ.NextVal from dual ").FirstOrDefault<Int32>();
            data.ind_inicio = string.IsNullOrEmpty(data.ind_inicio) ? "N" : data.ind_inicio;
            data.ind_liberado = string.IsNullOrEmpty(data.ind_liberado) ? "N" : data.ind_liberado;


            TryValidateModel(data);


            if (ModelState.IsValid)
            {
                db.Estagio.Add(data);
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

            Estagio data = db.Estagio.Find(id);
            if (data == null)
            {
                return InvokeHttpNotFound();
            }

            ViewBag.statusid = new SelectList(db.Status, "statusId", "descricao", data.statusid);
            ViewBag.estagioid_proximo = new SelectList(db.Estagio, "estagioId", "descricao", data.estagioid_proximo);
            ViewBag.estagioid_anterior = new SelectList(db.Estagio, "estagioId", "descricao", data.estagioid_anterior);

            return View(data);
        }
        //
        // POST: /TipoLead/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(Estagio data, bool continueAdd, bool isDelete)
        {
            ViewBag.statusid = new SelectList(db.Status, "statusId", "descricao", data.statusid);
            ViewBag.estagioid_proximo = new SelectList(db.Estagio, "estagioId", "descricao", data.estagioid_proximo);
            ViewBag.estagioid_anterior = new SelectList(db.Estagio, "estagioId", "descricao", data.estagioid_anterior);



            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.estagioId }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    Estagio dataDelete = db.Estagio.Find(data.estagioId);
                    db.Estagio.Remove(dataDelete);
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
                    return RedirectToAction("Edit", new { id = data.estagioId });
                }
                return RedirectToAction("List");
            }
        }







    }
}
