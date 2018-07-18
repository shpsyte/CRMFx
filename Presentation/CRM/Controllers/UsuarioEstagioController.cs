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
    public class UsuarioEstagioController : BasePublicController
    {
        //
        // GET: /EstagioUsuario/

        public ActionResult List(int id)
        {
            var data = new UsuarioEstagio_Model
            {
                ListaUsuario = db.EstagioUsuario.Where(a => a.estagioId == id).ToList(),
                cod_estagio = id,
                nome_estagio = db.Estagio.Where(a => a.estagioId == id).Select(a => a.descricao).FirstOrDefault()
            };
            return View(data);
        }


        public ActionResult Create(int id)
        {
            var data = new UsuarioEstagio_Model
            {
                cod_estagio = id,
                nome_estagio = db.Estagio.Where(a => a.estagioId == id).Select(a => a.descricao).FirstOrDefault(),
                ListaUsuario = db.EstagioUsuario.Where(a => a.estagioId == id).ToList()
            };

            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome");

            return View(data);
        }
        //
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(UsuarioEstagio_Model data, bool continueAdd, FormCollection form)
        {
            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome", data.EstagioUsuario.cd_usuario);
            ModelState.Clear();
            data.EstagioUsuario.estagiousuarioId = db.Database.SqlQuery<Int32>("select EstagioUsuarioSeq.NextVal from dual ").FirstOrDefault<Int32>();
            data.EstagioUsuario.estagioId = data.cod_estagio;
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.EstagioUsuario.Add(data.EstagioUsuario);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create", new { id = data.cod_estagio }) : RedirectToAction("List", new { id = data.cod_estagio });
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id, int cod_estagio)
        {
            var data = new UsuarioEstagio_Model
            {
                cod_estagio = cod_estagio,
                nome_estagio = db.Estagio.Where(a => a.estagioId == id).Select(a => a.descricao).FirstOrDefault(),
                EstagioUsuario = db.EstagioUsuario.Find(id)
            };



            if (data.EstagioUsuario == null)
            {
                return InvokeHttpNotFound();
            }
            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome", data.EstagioUsuario.cd_usuario);
            return View(data);
        }
        //
        // POST: /TipoLead/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(UsuarioEstagio_Model data, bool continueAdd, bool isDelete)
        {
            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome", data.EstagioUsuario.cd_usuario);
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data.EstagioUsuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.EstagioUsuario.estagioId }) : RedirectToAction("List", new { id = data.EstagioUsuario.estagioId });
                }
                return View(data);
            }
            else
            {
                try
                {
                    EstagioUsuario dataDelete = db.EstagioUsuario.Find(data.EstagioUsuario.estagiousuarioId);
                    db.EstagioUsuario.Remove(dataDelete);
                    db.SaveChanges();
                    RedirectToAction("List", new { id = data.cod_estagio });
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
                    return RedirectToAction("Edit", new { id = data.EstagioUsuario.estagiousuarioId, campanhaorcamentoid = data.EstagioUsuario.estagioId });
                }
                return RedirectToAction("List", new { id = data.cod_estagio });
            }
        }


    }
}
