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
    public class CampanhaOrcamentoRegionaisController : BasePublicController
    {
        //
        // GET: /EstagioUsuario/

        public ActionResult List(int id)
        {
            var data = new CampanhaOrcamentoRegionalModel
            {
                ListaRegional = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == id).ToList(),
                campanhaorcamentoid = id,
                vlr_orcamento = (from a in db.CampanhaOrcamento where a.ano == id select (decimal)a.vlr_orcamento).FirstOrDefault()
        };
            return View(data);
        }


        public ActionResult Create(int id)
        {
            var data = new CampanhaOrcamentoRegionalModel
            {
                campanhaorcamentoid = id,
                ListaRegional = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == id).ToList(),
                vlr_orcamento = (from a in db.CampanhaOrcamento where a.campanhaorcamentoid == id select (decimal)a.vlr_orcamento).FirstOrDefault()
            };

            var listaregionaldoano = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == id).Select(a => a.cod_regional).ToList();
            ViewBag.cod_regional = new SelectList(db.GeUnidades.Where(a => !listaregionaldoano.Contains(a.cod_unidade)).OrderBy(a => a.cod_unidade), "cod_unidade", "des_nome");




            decimal vlr_ja_orcado = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == id).Select(a => a.vlr_orcamento).DefaultIfEmpty(0).Sum();

            ViewBag.ValorRestante = (data.vlr_orcamento - vlr_ja_orcado).ToString("C");

            return View(data);
        }
        //
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(CampanhaOrcamentoRegionalModel data, bool continueAdd, FormCollection form)
        {
            decimal vlr_ja_orcado = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == data.campanhaorcamentoid).Select(a => a.vlr_orcamento).DefaultIfEmpty(0).Sum();
            ViewBag.ValorRestante = (data.vlr_orcamento - vlr_ja_orcado).ToString("C");


            var listaregionaldoano = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == data.campanhaorcamentoid).Select(a => a.cod_regional).ToList();
            ViewBag.cod_regional = new SelectList(db.GeUnidades.Where(a => !listaregionaldoano.Contains(a.cod_unidade)).OrderBy(a => a.cod_unidade), "cod_unidade", "des_nome", data.CampanhaOrcamentoRegional.cod_regional);

            ModelState.Clear();
            data.CampanhaOrcamentoRegional.campanhaorcamentoregionalid = db.Database.SqlQuery<Int32>("select CampanhaOrcamentoRegionalSeq.NextVal from dual ").FirstOrDefault<Int32>();
            data.CampanhaOrcamentoRegional.campanhaorcamentoid = data.campanhaorcamentoid;
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.CampanhaOrcamentoRegional.Add(data.CampanhaOrcamentoRegional);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create", new { id = data.campanhaorcamentoid }) : RedirectToAction("List", new { id = data.campanhaorcamentoid });
            }
            return View(data);
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id, int campanhaorcamentoid)
        {
            decimal vlr_ja_orcado = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == campanhaorcamentoid).Select(a => a.vlr_orcamento).DefaultIfEmpty(0).Sum();
            decimal vrl_total = db.CampanhaOrcamento.Where(a => a.campanhaorcamentoid == campanhaorcamentoid).Select(a => a.vlr_orcamento).DefaultIfEmpty(0).Sum();
            ViewBag.ValorRestante = (vrl_total - vlr_ja_orcado).ToString("C");
            var data = new CampanhaOrcamentoRegionalModel
            {
                campanhaorcamentoid = campanhaorcamentoid,
                CampanhaOrcamentoRegional = db.CampanhaOrcamentoRegional.Find( id, campanhaorcamentoid)
            };

            if (data.CampanhaOrcamentoRegional == null)
            {
                return InvokeHttpNotFound();
            }
            ViewBag.cod_regional = new SelectList(db.GeUnidades.OrderBy(a => a.cod_unidade), "cod_unidade", "des_nome", data.CampanhaOrcamentoRegional.cod_regional);
            return View(data);
        }
        //
        // POST: /TipoLead/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(CampanhaOrcamentoRegionalModel data, bool continueAdd, bool isDelete)
        {
            decimal vlr_ja_orcado = db.CampanhaOrcamentoRegional.Where(a => a.campanhaorcamentoid == data.campanhaorcamentoid).Select(a => a.vlr_orcamento).DefaultIfEmpty(0).Sum();
            decimal vrl_total = db.CampanhaOrcamento.Where(a => a.campanhaorcamentoid == data.campanhaorcamentoid).Select(a => a.vlr_orcamento).DefaultIfEmpty(0).Sum();
            ViewBag.ValorRestante = (vrl_total - vlr_ja_orcado).ToString("C");

            ViewBag.cod_regional = new SelectList(db.GeUnidades.OrderBy(a => a.cod_unidade), "cod_unidade", "des_nome", data.CampanhaOrcamentoRegional.cod_regional);

            bool ja_existe = db.CampanhaOrcamentoRegional
                .Where(a => a.campanhaorcamentoid  == data.campanhaorcamentoid 
                         && a.cod_regional == data.CampanhaOrcamentoRegional.cod_regional
                         && a.campanhaorcamentoregionalid != data.CampanhaOrcamentoRegional.campanhaorcamentoregionalid).Count() > 0;
            if (ja_existe)
            {
                ModelState.AddModelError("cod_regional", "Já existe um orçamento para esta regional neste ano!");
                return View(data);
            }



            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data.CampanhaOrcamentoRegional).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.CampanhaOrcamentoRegional.campanhaorcamentoregionalid }) : RedirectToAction("List", new { id = data.campanhaorcamentoid });
                }
                return View(data);
            }
            else
            {
                try
                {
                    CampanhaOrcamentoRegional dataDelete = db.CampanhaOrcamentoRegional.Find(data.CampanhaOrcamentoRegional.campanhaorcamentoregionalid, data.CampanhaOrcamentoRegional.campanhaorcamentoid);
                    db.CampanhaOrcamentoRegional.Remove(dataDelete);
                    db.SaveChanges();
                    RedirectToAction("List", new { id = data.campanhaorcamentoid });
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
                    return RedirectToAction("Edit", new { id = data.CampanhaOrcamentoRegional.campanhaorcamentoregionalid, campanhaorcamentoid = data.CampanhaOrcamentoRegional.campanhaorcamentoid });
                }
                return RedirectToAction("List", new { id = data.campanhaorcamentoid });
            }
        }




    }
}
