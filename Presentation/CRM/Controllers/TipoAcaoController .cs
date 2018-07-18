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
    public class TipoAcaoController : BasePublicController
    {

        //
        // GET: /TipoLead/
        public ActionResult List(int id)
        {
            var data = new TipoAcao_Model
            {
                ListaTipoAcao = db.TipoAcao.Where(a => a.segmentoid == id).ToList(),
                CodSegmento = id,
                NomeSegmento = db.Segmentos.Where(a => a.segmentoid == id).Select(a => a.des_segmento).FirstOrDefault()
            };
                
            return View(data);
        }
        //
        // GET: /TipoLead/Details/5
        //
        // GET: /TipoLead/Create
        public ActionResult Create(int id)
        {
            var data = new TipoAcao_Model
            {
                CodSegmento = id,
                NomeSegmento = db.Segmentos.Where(a => a.segmentoid == id).Select(a => a.des_segmento).FirstOrDefault()

            };
            return View(data);
        }
        //
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(TipoAcao_Model data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.TipoAcao.tipoacaoid = db.Database.SqlQuery<Int32>("select Tipoacao_seq.NextVal from dual ").FirstOrDefault<Int32>();
            data.TipoAcao.segmentoid = data.CodSegmento;
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.TipoAcao.Add(data.TipoAcao);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create", new { id = data.CodSegmento }) : RedirectToAction("List", new { id = data.CodSegmento });
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id, int cod_segmento)
        {
            var data = new TipoAcao_Model
            {
                TipoAcao = db.TipoAcao.Find(id, cod_segmento),
                CodSegmento = cod_segmento,
                NomeSegmento = db.Segmentos.Where(a => a.segmentoid == cod_segmento).Select(a => a.des_segmento).FirstOrDefault()

            };

            if (data.TipoAcao == null)
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
        public ActionResult Edit(TipoAcao_Model data, bool continueAdd, bool isDelete)
        {
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data.TipoAcao).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.TipoAcao.tipoacaoid , cod_segmento = data.TipoAcao.segmentoid }) : RedirectToAction("List", new { id = data.TipoAcao.segmentoid });
                }
                return View(data);
            }
            else
            {
                try
                {
                    TipoAcao dataDelete = db.TipoAcao.Find(data.TipoAcao.tipoacaoid, data.TipoAcao.segmentoid);
                    db.TipoAcao.Remove(dataDelete);
                    db.SaveChanges();
                    RedirectToAction("List", new { id = data.CodSegmento });
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
                    return RedirectToAction("Edit", new { id = data.TipoAcao.tipoacaoid,  cod_segmento = data.TipoAcao.segmentoid });
                }
                return RedirectToAction("List", new { id = data.TipoAcao.segmentoid });
            }



        }







    }
}
