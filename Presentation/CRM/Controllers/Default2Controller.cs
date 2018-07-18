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
    public class Default2Controller : Controller
    {
        private b2yweb_entities db = new b2yweb_entities();

        //
        // GET: /Default2/

        public ActionResult Index()
        {
            var listacomentarios = db.ListaComentarios.Include(n => n.Usuario);
            return View(listacomentarios.ToList());
        }

        //
        // GET: /Default2/Details/5

        public ActionResult Details(int id = 0)
        {
            Note note = db.ListaComentarios.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // GET: /Default2/Create

        public ActionResult Create()
        {
            ViewBag.cod_usuario = new SelectList(db.Usuario, "CD_USUARIO", "NOME");
            return View();
        }

        //
        // POST: /Default2/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {
                db.ListaComentarios.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cod_usuario = new SelectList(db.Usuario, "CD_USUARIO", "NOME", note.cod_usuario);
            return View(note);
        }

        //
        // GET: /Default2/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Note note = db.ListaComentarios.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.cod_usuario = new SelectList(db.Usuario, "CD_USUARIO", "NOME", note.cod_usuario);
            return View(note);
        }

        //
        // POST: /Default2/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cod_usuario = new SelectList(db.Usuario, "CD_USUARIO", "NOME", note.cod_usuario);
            return View(note);
        }

        //
        // GET: /Default2/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Note note = db.ListaComentarios.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // POST: /Default2/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.ListaComentarios.Find(id);
            db.ListaComentarios.Remove(note);
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