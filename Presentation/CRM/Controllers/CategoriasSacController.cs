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
    public class CategoriasSacController : BasePublicController
    {
        //
        // GET: /TipoLead/
        public ActionResult List(int id)
        {
            var data = new PS_Categorias_Sac_Model
            {
                ListaCategorias = db.PS_Categorias_Sac.Where(a => a.cod_classe == id).ToList(),
                cod_classe = id,
                nome_classe = db.PS_Classe_Sac.Where(a => a.cod_classe == id).Select(a => a.des_nome).FirstOrDefault()
            };
            return View(data);
        }
        //
        // GET: /TipoLead/Details/5
        //
        // GET: /TipoLead/Create
        public ActionResult Create(int id)
        {
            var data = new PS_Categorias_Sac_Model
              {
                  cod_classe = id,
                  nome_classe = db.PS_Classe_Sac.Where(a => a.cod_classe == id).Select(a => a.des_nome).FirstOrDefault()

              };

            return View(data);
        }
        //
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(PS_Categorias_Sac_Model data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.PS_Categorias_Sac.cod_categoria = db.Database.SqlQuery<Int32>("select PS_Categorias_Sac_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            data.PS_Categorias_Sac.cod_classe = data.cod_classe;
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.PS_Categorias_Sac.Add(data.PS_Categorias_Sac);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create", new { id = data.cod_classe }) : RedirectToAction("List", new { id = data.cod_classe });
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id, int cod_classe)
        {
            var data = new PS_Categorias_Sac_Model
             {
                 PS_Categorias_Sac = db.PS_Categorias_Sac.Find(id, cod_classe),
                 cod_classe = cod_classe,
                 nome_classe = db.PS_Classe_Sac.Where(a => a.cod_classe == cod_classe).Select(a => a.des_nome).FirstOrDefault()

             };

            if (data.PS_Categorias_Sac == null)
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
        public ActionResult Edit(PS_Categorias_Sac_Model data, bool continueAdd, bool isDelete)
        {
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data.PS_Categorias_Sac).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.PS_Categorias_Sac.cod_categoria, cod_classe = data.PS_Categorias_Sac.cod_classe }) : RedirectToAction("List", new { id = data.PS_Categorias_Sac.cod_classe });
                }
                return View(data);
            }
            else
            {
                try
                {
                    PS_Categorias_Sac dataDelete = db.PS_Categorias_Sac.Find(data.PS_Categorias_Sac.cod_categoria, data.cod_classe);
                    db.PS_Categorias_Sac.Remove(dataDelete);
                    db.SaveChanges();
                    RedirectToAction("List", new { id = data.cod_classe });
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
                    return RedirectToAction("Edit", new { id = data.PS_Categorias_Sac.cod_categoria });
                }
                return RedirectToAction("List", new { id = data.cod_classe });
            }
        }
    }
}
