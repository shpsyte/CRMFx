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
    public class EstagioUsuarioController : BasePublicController
    {
        //
        // GET: /EstagioUsuario/

        public ActionResult List(int id)
        {
            var data = new PS_Estagio_Usuario_Model
            {
                ListaUsuario = db.Ps_Estagio_Usuario.Where(a => a.cod_estagio == id).ToList(),
                cod_estagio = id,
                nome_estagio = db.PS_Estagio_Sac.Where(a => a.cod_estagio == id).Select(a => a.des_nome).FirstOrDefault()
            };
            return View(data);
        }


        public ActionResult Create(int id)
        {
            var data = new PS_Estagio_Usuario_Model
            {
                cod_estagio = id,
                nome_estagio = db.PS_Estagio_Sac.Where(a => a.cod_estagio == id).Select(a => a.des_nome).FirstOrDefault(),
                ListaUsuario = db.Ps_Estagio_Usuario.Where(a => a.cod_estagio == id).ToList()
            };

            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome");

            return View(data);
        }
        //
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(PS_Estagio_Usuario_Model data, bool continueAdd, FormCollection form)
        {
            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome", data.Ps_Estagio_Usuario.cd_usuario);
            ModelState.Clear();
            data.Ps_Estagio_Usuario.idrow = db.Database.SqlQuery<Int32>("select PS_ESTAGIO_USUARIO_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            data.Ps_Estagio_Usuario.cod_estagio = data.cod_estagio;
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.Ps_Estagio_Usuario.Add(data.Ps_Estagio_Usuario);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create", new { id = data.cod_estagio }) : RedirectToAction("List", new { id = data.cod_estagio });
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id, int cod_estagio)
        {
            var data = new PS_Estagio_Usuario_Model
            {
                cod_estagio = cod_estagio,
                nome_estagio = db.PS_Estagio_Sac.Where(a => a.cod_estagio == id).Select(a => a.des_nome).FirstOrDefault(),
                Ps_Estagio_Usuario = db.Ps_Estagio_Usuario.Find(id)
            };



            if (data.Ps_Estagio_Usuario == null)
            {
                return InvokeHttpNotFound();
            }
            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome", data.Ps_Estagio_Usuario.cd_usuario);
            return View(data);
        }
        //
        // POST: /TipoLead/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(PS_Estagio_Usuario_Model data, bool continueAdd, bool isDelete)
        {
            ViewBag.cd_usuario = new SelectList(db.Usuario.Where(p => p.SITUACAO == "S"), "cd_usuario", "nome", data.Ps_Estagio_Usuario.cd_usuario);
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data.Ps_Estagio_Usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.Ps_Estagio_Usuario.cod_estagio }) : RedirectToAction("List", new { id = data.Ps_Estagio_Usuario.cod_estagio });
                }
                return View(data);
            }
            else
            {
                try
                {
                    Ps_Estagio_Usuario dataDelete = db.Ps_Estagio_Usuario.Find(data.Ps_Estagio_Usuario.idrow);
                    db.Ps_Estagio_Usuario.Remove(dataDelete);
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
                    return RedirectToAction("Edit", new { id = data.Ps_Estagio_Usuario.idrow, campanhaorcamentoid = data.Ps_Estagio_Usuario.cod_estagio });
                }
                return RedirectToAction("List", new { id = data.cod_estagio });
            }
        }


    }
}
