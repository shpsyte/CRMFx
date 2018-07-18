using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Data.Context;

namespace CRM.Controllers
{
    public class testeController : Controller
    {
        private b2yweb_entities db = new b2yweb_entities();

        //
        // GET: /teste/

        public ActionResult Index()
        {
            return View(db.Cargos.ToList());
        }

        //
        // GET: /teste/Details/5

        public ActionResult Details(int id = 0)
        {
            Cargos cargos = db.Cargos.Find(id);
            if (cargos == null)
            {
                return HttpNotFound();
            }
            return View(cargos);
        }

        //
        // GET: /teste/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /teste/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cargos cargos)
        {
            if (ModelState.IsValid)
            {
                db.Cargos.Add(cargos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cargos);
        }

        //
        // GET: /teste/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cargos cargos = db.Cargos.Find(id);
            if (cargos == null)
            {
                return HttpNotFound();
            }
            return View(cargos);
        }

        //
        // POST: /teste/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cargos cargos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cargos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cargos);
        }

        //
        // GET: /teste/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cargos cargos = db.Cargos.Find(id);
            if (cargos == null)
            {
                return HttpNotFound();
            }
            return View(cargos);
        }

        //
        // POST: /teste/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cargos cargos = db.Cargos.Find(id);
            db.Cargos.Remove(cargos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}