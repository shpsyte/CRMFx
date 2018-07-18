using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Services.Functions;
using System.Data.Entity.Validation;
namespace CRM.Controllers
{
    [NoCache]
    public class ContactsController : BasePublicController
    {
        //
        // GET: /Contacts/
        public ActionResult List()
        {
            var contacts = db.Contatos.ToList();
            return View(contacts);
        }
        //
        // GET: /Contacts/Create
        public ActionResult Create(string Accountid = "")
        {
            ViewBag.cod_conta = new SelectList(db.Dados_crm.ToList().OrderBy(p => p.des_fantasia), "cod_pessoa", "FullName", Accountid);
            ViewBag.cod_cargo = new SelectList(db.Cargos, "cod_cargo", "des_cargo");
            ViewBag.cod_estado_civil = new SelectList(db.Combo.Where(p => p.TIPO == 5), "Value", "Text");
            return View();
        }
        //
        // POST: /Contacts/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(Contatos contatos, bool continueAdd, FormCollection form)
        {
            ModelState.Clear();
            contatos.cod_contato = db.Database.SqlQuery<Int32>("select ps_Contatos_Crm_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            TryValidateModel(contatos);
            if (ModelState.IsValid)
            {
                db.Contatos.Add(contatos);
                db.SaveChanges();
                return continueAdd ? RedirectToAction("Create", new { Accountid = contatos.cod_conta }) : RedirectToAction("List");
            }
            ViewBag.cod_conta = new SelectList(db.Dados_crm.ToList(), "cod_pessoa", "FullName", contatos.cod_conta);
            ViewBag.cod_cargo = new SelectList(db.Cargos, "cod_cargo", "des_cargo", contatos.cod_cargo);
            ViewBag.cod_estado_civil = new SelectList(db.Combo.Where(p => p.TIPO == 5), "Value", "Text", contatos.cod_estado_civil);
            return View(contatos);
        }
        //
        // GET: /Contacts/Edit/5
        public ActionResult Edit(int id)
        {
            Contatos contatos = db.Contatos.Find(id);
            if (contatos == null)
            {
                return InvokeHttpNotFound();
            }
            ViewBag.cod_conta = new SelectList(db.Dados_crm.ToList(), "cod_pessoa", "FullName", contatos.cod_conta);
            ViewBag.cod_cargo = new SelectList(db.Cargos, "cod_cargo", "des_cargo", contatos.cod_cargo);
            ViewBag.cod_estado_civil = new SelectList(db.Combo.Where(p => p.TIPO == 5), "Value", "Text", contatos.cod_estado_civil);
            return View(contatos);
        }
        //
        // POST: /Contacts/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(Contatos contatos, bool continueAdd, bool isDelete)
        {
            ViewBag.cod_conta = new SelectList(db.Dados_crm.ToList(), "cod_pessoa", "FullName", contatos.cod_conta);
            ViewBag.cod_cargo = new SelectList(db.Cargos, "cod_cargo", "des_cargo", contatos.cod_cargo);
            ViewBag.cod_estado_civil = new SelectList(db.Combo.Where(p => p.TIPO == 5), "Value", "Text", contatos.cod_estado_civil);
            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contatos).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = contatos.cod_contato }) : RedirectToAction("List");
                }
                return View(contatos);
            }
            else
            {
                try
                {
                    Contatos contatoDelete = db.Contatos.Find(contatos.cod_contato);
                    db.Contatos.Remove(contatoDelete);
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
                    return RedirectToAction("Edit", new { id = contatos.cod_contato });
                }
                return RedirectToAction("List");
            }
        }
    }
}
