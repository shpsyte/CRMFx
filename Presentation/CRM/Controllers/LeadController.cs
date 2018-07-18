using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CRM.Controllers
{
    public class LeadController : BasePublicController
    {
        //
        // GET: /TipoLead/
        public ActionResult List()
        {
            var data = db.Ps_Leads.Include("Ps_Situacao_Lead").ToList();
            return View(data);
        }


        public void Excel()
        {

            var leads = db.Ps_Leads.ToList();

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in leads select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=procedimentoadm.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();


        }
        //
        // GET: /TipoLead/Details/5
        //
        // GET: /TipoLead/Create
        public ActionResult Create()
        {
            ViewBag.cod_tipo = new SelectList(db.Ps_Tipo_Lead.ToList(), "cod_tipo", "des_nome");
            ViewBag.cod_origem = new SelectList(db.Ps_Origem_Lead.ToList(), "cod_origem", "des_nome", 1);
            ViewBag.cod_situacao = new SelectList(db.Ps_Situacao_Lead.ToList(), "cod_situacao", "des_nome", 1);
            ViewBag.des_interesse = new SelectList(db.Ps_Interesse_Lead.ToList(), "cod_interesse", "des_nome");
            return View();
        }
        //
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(Ps_Leads data, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            data.cod_interesse = ReturnNullIfZero(data.cod_interesse);
            //data.cod_origem = ReturnNullIfZero(data.cod_origem);
            data.cod_tipo = ReturnNullIfZero(data.cod_tipo);
            data.cod_situacao = ReturnNullIfZero(data.cod_situacao);
            data.dta_criado = System.DateTime.Now;
            data.cod_lead = db.Database.SqlQuery<Int32>("select Ps_Leads_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.Ps_Leads.Add(data);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create", "SAC", new { tipo_pessoa = "L", cod_pessoa = data.cod_lead }) : RedirectToAction("List");
            }
            ViewBag.cod_tipo = new SelectList(db.Ps_Tipo_Lead.ToList(), "cod_tipo", "des_nome", data.cod_tipo);
            ViewBag.cod_origem = new SelectList(db.Ps_Origem_Lead.ToList(), "cod_origem", "des_nome", data.cod_origem);
            ViewBag.cod_situacao = new SelectList(db.Ps_Situacao_Lead.ToList(), "cod_situacao", "des_nome", data.cod_situacao);
            ViewBag.des_interesse = new SelectList(db.Ps_Interesse_Lead.ToList(), "cod_interesse", "des_nome", data.cod_interesse);
            return View(data);
        }

        public  JsonResult InsertCustomer(string tipo, string nome)
        {

            return Json(new { codigo = 1} );
        }


        private int? ReturnNullIfZero(int? p)
        {
            if (p == 0 || p == null)
            {
                return null;
            }
            else
                return p;
        }
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id)
        {
            Ps_Leads data = db.Ps_Leads.Find(id);
            if (data == null)
            {
                return InvokeHttpNotFound();
            }
            ViewBag.cod_tipo = new SelectList(db.Ps_Tipo_Lead.ToList(), "cod_tipo", "des_nome", data.cod_tipo);
            ViewBag.cod_origem = new SelectList(db.Ps_Origem_Lead.ToList(), "cod_origem", "des_nome", data.cod_origem);
            ViewBag.cod_situacao = new SelectList(db.Ps_Situacao_Lead.ToList(), "cod_situacao", "des_nome", data.cod_situacao);
            //ViewBag.cod_interesse = new SelectList(db.Ps_Interesse_Lead.ToList(), "cod_interesse", "des_nome", data.cod_interesse);
            ViewBag.des_interesse = new SelectList(db.Ps_Interesse_Lead.ToList(), "cod_interesse", "des_nome");

            string ids = id.ToString();
            ViewBag.comentarios = db.ListaComentarios.Where(a => a.cod_interno == ids && a.tipo_nota == "LEADS").ToList();

            return View(data);
        }
        //
        // POST: /TipoLead/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(Ps_Leads data, bool continueAdd, bool isDelete, string[] des_interesse)
        {
            string ids = data.cod_lead.ToString();
            ViewBag.cod_tipo = new SelectList(db.Ps_Tipo_Lead.ToList(), "cod_tipo", "des_nome", data.cod_tipo);
            ViewBag.cod_origem = new SelectList(db.Ps_Origem_Lead.ToList(), "cod_origem", "des_nome", data.cod_origem);
            ViewBag.cod_situacao = new SelectList(db.Ps_Situacao_Lead.ToList(), "cod_situacao", "des_nome", data.cod_situacao);
            ViewBag.cod_interesse = new SelectList(db.Ps_Interesse_Lead.ToList(), "cod_interesse", "des_nome", data.cod_interesse);
            ViewBag.des_interesse = new SelectList(db.Ps_Interesse_Lead.ToList(), "cod_interesse", "des_nome");
            ViewBag.comentarios = db.ListaComentarios.Where(a => a.cod_interno == ids && a.tipo_nota == "LEADS").ToList();

            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.cod_lead }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    
                    Ps_Leads dataDelete = db.Ps_Leads.Find(data.cod_lead);
                    db.Ps_Leads.Remove(dataDelete);
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
                    return RedirectToAction("Edit", new { id = data.cod_lead });
                }
                return RedirectToAction("List");
            }
        }
    }
}
