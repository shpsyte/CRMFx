
using CRM.App_Helpers;
using CRM.DataServices;
using CRM.Extends;
using CRM.Models;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class SacController : BasePublicController
    {
        private PS_SacDataServices _sacDataServices;
        private PS_Situacao_SacDataServices _situacaoSacDataServices;
        private EstagioSacServices _estagioSacServices;
        private readonly IDataServices<PS_Sac> _sacNewDataServices;
        private readonly IDataServices<Ps_Sac_Ps_Sac> _sacSacDataServices;
        private readonly IDataServices<Ps_Sac_Reabertura> _sacps_sac_reaberturaServices;


        public SacController(PS_SacDataServices sacDataServices, 
                             PS_Situacao_SacDataServices situacaoSacDataServices,
                             EstagioSacServices estagioSacServices,
                             IDataServices<PS_Sac> sacNewDataServices,
                             IDataServices<Ps_Sac_Ps_Sac> sacSacDataServices,
                             IDataServices<Ps_Sac_Reabertura> sacps_sac_reaberturaServices)
        {
            this._sacDataServices = sacDataServices;
            this._situacaoSacDataServices = situacaoSacDataServices;
            this._estagioSacServices = estagioSacServices;
            this._sacNewDataServices = sacNewDataServices;
            this._sacSacDataServices = sacSacDataServices;
            this._sacps_sac_reaberturaServices = sacps_sac_reaberturaServices;

        }
        //
        // GET: /TipoLead/


        //public ActionResult List(bool ind_aberta = true, bool ind_meus = false, bool ind_ag = false)
        //{


        //    ViewBag.situacao = new SelectList(db.PS_Situacao_Sac, "cod_situacao", "des_nome");
        //    ViewBag.estagio = new SelectList(db.PS_Estagio_Sac, "cod_estagio", "des_nome");


        //    return View();


        //    //var allItem = db.PS_Sac.ToList();
        //    int cod_final = int.Parse(Settings.cod_situacao_finalizacao);
        //    int cod_cancelada = int.Parse(Settings.cod_situacao_cancelada);

        //    var Lista_Estagio = (List<int>)db.Ps_Estagio_Usuario.Where(a => a.cd_usuario == cd_usuario).Select(a => a.cod_estagio).ToList();


        //    IEnumerable<PS_Sac> filtersac;

        //    if (ind_aberta)
        //    {
        //        if (ind_meus)
        //        {
        //            filtersac = db.PS_Sac.Where(c => Lista_Estagio.Contains((int)c.cod_estagio) && (c.cod_situacao != cod_final && c.cod_situacao != cod_cancelada) && (c.cod_estagio != 2));
        //            //filtersac = db.PS_Sac.Where(c => c.cod_situacao == 41 && (c.cod_situacao != cod_final && c.cod_situacao != cod_cancelada ));
        //        }
        //        else
        //        {
        //            filtersac = db.PS_Sac.Where(c => (c.cod_situacao != cod_final && c.cod_situacao != cod_cancelada && c.cod_estagio != 1));
        //        }
        //    }
        //    else
        //    {

        //        if (ind_meus)
        //        {
        //            filtersac = db.PS_Sac.Where(c => Lista_Estagio.Contains((int)c.cod_estagio) && (c.cod_situacao == cod_final || c.cod_situacao == cod_cancelada));
        //        }
        //        else
        //        {
        //            filtersac = db.PS_Sac.Where(c => (c.cod_situacao == cod_final || c.cod_situacao == cod_cancelada || c.cod_estagio == 1));
        //        }


        //    }


        //    var data = filtersac.ToList().OrderByDescending(p => p.cod_sac);
        //    return View(data);


        //}
        ////
        //// GET: /TipoLead/Details/5
        ////
        //// GET: /TipoLead/Create




        //public ActionResult ReadWorkFlow(ParametersDataTable param)
        //{

        //    string situacao = Convert.ToString(Request["sSearch_3"]);
        //    string estagio = Convert.ToString(Request["sSearch_4"]);
        //    string cliente = Convert.ToString(Request["sSearch_1"]);

        //    //exclusao
        //    List<int> listIDs = new List<int> { 2, 3 };


        //    string _count = " select count(*)  " +
        //                                         " from ps_sac a " +
        //                                         " Inner join ps_situacao_sac b on a.cod_situacao = b.cod_situacao " +
        //                                         " Inner join ps_estagio_sac c on a.cod_estagio = c.cod_estagio " +
        //                                         " Inner join Ps_Pessoas_Sac d on d.TIPO = a.tp_pessoa and d.COD_PESSOA = a.cod_pessoa " +
        //                                         " Where 1 = 1 ";


        //    if (!string.IsNullOrEmpty(situacao))
        //        _count = _count + string.Format(" And b.des_nome = \'{0}\' ", situacao);
        //    else
        //        _count = _count + string.Format(" And b.cod_situacao not in ({0}) ", "2,3");


        //    if (!string.IsNullOrEmpty(estagio))
        //        _count = _count + string.Format(" And c.des_nome = \'{0}\' ", estagio);

        //    if (!string.IsNullOrEmpty(cliente))
        //        _count = _count + string.Format(" And d.DES_PESSOA = \'{0}\' or d.DES_FANTASIA = \'{1}\' ", cliente, cliente);


        //    int total_de_linhas = db.Database.SqlQuery<int>(_count).FirstOrDefault();



        //    IEnumerable<PS_Sac> filteredCompanies;
        //    filteredCompanies = db.PS_Sac;

        //    var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


        //    if (!string.IsNullOrEmpty(situacao))
        //    {
        //        filteredCompanies = filteredCompanies.Where(c => c.PS_Situacao_Sac.des_nome.ToUpper().Contains(situacao.ToUpper()));
        //    }
        //    else
        //    {
        //        filteredCompanies = filteredCompanies.Where(c => !listIDs.Contains((int)c.cod_situacao));
        //    }


        //    if (!string.IsNullOrEmpty(estagio))
        //        filteredCompanies = filteredCompanies.Where(c => c.PS_Situacao_Sac.des_nome.ToUpper().Contains(estagio.ToUpper()));

        //    if (!string.IsNullOrEmpty(cliente))
        //        filteredCompanies = filteredCompanies.Where(c => c.PS_Pessoas_Sac.des_pessoa.ToUpper().Contains(cliente.ToUpper()) || c.PS_Pessoas_Sac.des_fantasia.ToUpper().Contains(cliente.ToUpper()));


        //    var displayedCompanies = filteredCompanies
        //                 .Skip(param.iDisplayStart)
        //                 .Take(param.iDisplayLength);

        //    var result = from c in displayedCompanies
        //                 select new
        //                 {
        //                     sac_id = c.cod_sac,
        //                     des_pessoa = c.PS_Pessoas_Sac.des_pessoa,
        //                     des_fantasia = c.PS_Pessoas_Sac.des_fantasia,
        //                     des_assunto = c.des_assunto,
        //                     des_situacao = c.PS_Situacao_Sac.des_nome,
        //                     des_estagio = c.PS_Estagio_Sac.des_nome,
        //                     acao = ""
        //                 };
        //    JsonResult jsonResult = Json(new
        //    {
        //        sEcho = param.sEcho,
        //        iTotalRecords = total_de_linhas,
        //        iTotalDisplayRecords = filteredCompanies.Count(),
        //        aaData = result
        //    },
        //    JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}


        public ActionResult ReadSac(ParametersDataTable param)
        {
            int total_de_linhas = 0;
            var Query = _sacDataServices.Query();
            //var Query = db.PS_Sac.AsEnumerable();

            var situacao = Convert.ToString(Request["sSearch_10"]);
            var estagio = Convert.ToString(Request["sSearch_1"]);

            if (string.IsNullOrEmpty(situacao))
                situacao = "Abertos";

            int cod_final = int.Parse(Settings.cod_situacao_finalizacao);
            int cod_cancelada = int.Parse(Settings.cod_situacao_cancelada);


            
            if (situacao.Equals("Abertos"))
                total_de_linhas = _sacDataServices.Count(a => (a.cod_situacao != cod_final && a.cod_situacao != cod_cancelada && a.cod_estagio != 1));
            else if (situacao.Equals("Fechados"))
                total_de_linhas = _sacDataServices.Count(a => (a.cod_situacao == cod_final || a.cod_situacao == cod_cancelada || a.cod_estagio != 1));
            else if (situacao.Equals("Aguardando SAC/GARANTIA"))
                total_de_linhas = _sacDataServices.Count(a => a.cod_situacao == 1);
            else
                total_de_linhas = _sacDataServices.Count();



            if (situacao.Equals("Abertos"))
                Query = Query.Where(a => (a.cod_situacao != cod_final && a.cod_situacao != cod_cancelada && a.cod_estagio != 1));
            else if (situacao.Equals("Fechados"))
                Query = Query.Where(a => (a.cod_situacao == cod_final || a.cod_situacao == cod_cancelada || a.cod_estagio == 1));
            else if (situacao.Equals("Aguardando SAC/GARANTIA"))
                Query = Query.Where(a => a.cod_situacao == 1);

            string codigo = Request["sSearch_0"];
            string nome = Request["sSearch_2"];
            string fantasia = Request["sSearch_4"];
            string assunto = Request["sSearch_5"];
            string email = Request["sSearch_7"];
            string tags = Request["sSearch_8"];

            int codint = 0;
            if (!string.IsNullOrEmpty(codigo))
                codint =  Convert.ToInt32(codigo);

            if (codint > 0)
                Query = Query.Where(a => a.cod_sac == codint);

            if (!string.IsNullOrEmpty(nome))
            { 
                Query = Query.Where(a => 
                       a.PS_Pessoas_Sac.des_pessoa.ToUpper().Contains(nome.ToUpper())
                  );
            }

            if (!string.IsNullOrEmpty(email))
            {
                Query = Query.Where(a => a.PS_Pessoas_Sac.des_email_cli != null && a.PS_Pessoas_Sac.des_email_cli.ToUpper().Contains(email.ToUpper()) );
            }


            if (!string.IsNullOrEmpty(tags))
            {
                Query = Query.Where(a => a.tag != null && a.tag.ToUpper().Contains(tags.ToUpper()));
            }
            

            if (!string.IsNullOrEmpty(fantasia))
                Query = Query.Where(a => a.PS_Pessoas_Sac.des_fantasia.ToUpper().Contains(fantasia.ToUpper()));

            if (!string.IsNullOrEmpty(assunto))
                Query = Query.Where(a => a.des_assunto.ToUpper().Contains(assunto.ToUpper()));

            if (!string.IsNullOrEmpty(estagio))
                Query = Query.Where(a => a.PS_Estagio_Sac.des_nome.ToUpper().Contains(estagio.ToUpper()));

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PS_Sac, string> orderingFunction = (c =>
                                                           sortColumnIndex == 0 ? c.cod_sac.ToString() :
                                                           sortColumnIndex == 1 ? c.dta_abertura.ToString() :
                                                           sortColumnIndex == 2 ? c.PS_Pessoas_Sac.des_pessoa.ToString() :
                                                           sortColumnIndex == 3 ? c.cod_pessoa.ToString() :
                                                           sortColumnIndex == 4 ? c.PS_Pessoas_Sac.des_fantasia.ToString() :
                                                           sortColumnIndex == 5 ? c.des_assunto.ToString() :
                                                           sortColumnIndex == 6 ? c.PS_Estagio_Sac.des_nome :
                                                           sortColumnIndex == 7 ? c.PS_Pessoas_Sac.des_email_cli :
                                                           sortColumnIndex == 8 ? c.tag :
                                                           c.dta_abertura.ToString()
                                                         );

            var sortDirection = Request["sSortDir_0"]; // asc or desc


            var filteredCustomer = Query;//.ToList();

           // if (sortDirection == "asc")
          //      filteredCustomer = filteredCustomer.OrderBy(orderingFunction);
          //  else
          //      filteredCustomer = filteredCustomer.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCustomer.OrderByDescending(orderingFunction)
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength).ToList();



            var result = from c in displayedCompanies
                         select new
                         {
                             Id = c.cod_sac,
                             Nome = c.PS_Pessoas_Sac.des_pessoa,
                             Fantasia = c.PS_Pessoas_Sac.des_fantasia,
                             Assunto = c.des_assunto,
                             Estagio = c.PS_Estagio_Sac.des_nome,
                             DataAbertura = c.dta_abertura.Value.ToShortDateString(),
                             CodCliente = c.cod_pessoa,
                             Email = c.PS_Pessoas_Sac.des_email_cli,
                             Detalhar = "",
                             Responder = "",
                             Tags = c.tag
                         };

            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total_de_linhas,
                iTotalDisplayRecords = filteredCustomer.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;



        }



        public ActionResult List(bool ind_aberta = true, bool ind_meus = false, bool ind_ag = false)
        {
            //var que = _sacDataServices.Query(); 
                //que = que.Where(a => a.cod_sac >= 100);
            //que = que.Where(a => a.cod_usuario >= 1);

            //            var toto = que.ToList();

            ViewBag.Situacao = new SelectList(_situacaoSacDataServices.GetTipos(), "Id", "Nome", string.Empty);
            ViewBag.Estagio = new SelectList(_estagioSacServices.GetAll(), "cod_estagio", "des_nome", string.Empty);
            //var allItem = db.PS_Sac.ToList();

            return View();



            int cod_final = int.Parse(Settings.cod_situacao_finalizacao);
            int cod_cancelada = int.Parse(Settings.cod_situacao_cancelada);

            var Lista_Estagio = (List<int>)db.Ps_Estagio_Usuario.Where(a => a.cd_usuario == cd_usuario).Select(a => a.cod_estagio).ToList();


            IEnumerable<PS_Sac> filtersac;

            if (ind_aberta)
            {
                if (ind_meus)
                {
                    filtersac = db.PS_Sac.Where(c => Lista_Estagio.Contains((int)c.cod_estagio) && (c.cod_situacao != cod_final && c.cod_situacao != cod_cancelada) && (c.cod_estagio != 2));
                    //filtersac = db.PS_Sac.Where(c => c.cod_situacao == 41 && (c.cod_situacao != cod_final && c.cod_situacao != cod_cancelada ));
                }
                else
                {
                    filtersac = db.PS_Sac.Where(c => (c.cod_situacao != cod_final && c.cod_situacao != cod_cancelada && c.cod_estagio != 1));
                }
            }
            else
            {

                if (ind_meus)
                {
                    filtersac = db.PS_Sac.Where(c => Lista_Estagio.Contains((int)c.cod_estagio) && (c.cod_situacao == cod_final || c.cod_situacao == cod_cancelada));
                }
                else
                {
                    filtersac = db.PS_Sac.Where(c => (c.cod_situacao == cod_final || c.cod_situacao == cod_cancelada || c.cod_estagio == 1));
                }


            }


            var data = filtersac.ToList().OrderByDescending(p => p.cod_sac);

            return View(data);


        }
        //
        // GET: /TipoLead/Details/5
        //
        // GET: /TipoLead/Create




        public ActionResult ReadWorkFlow(ParametersDataTable param)
        {

            string situacao = Convert.ToString(Request["sSearch_3"]);
            string estagio = Convert.ToString(Request["sSearch_4"]);
            string cliente = Convert.ToString(Request["sSearch_1"]);




            //exclusao
            List<int> listIDs = new List<int> { 2, 3 };


            string _count = " select count(*)  " +
                                                 " from ps_sac a " +
                                                 " Inner join ps_situacao_sac b on a.cod_situacao = b.cod_situacao " +
                                                 " Inner join ps_estagio_sac c on a.cod_estagio = c.cod_estagio " +
                                                 " Inner join Ps_Pessoas_Sac d on d.TIPO = a.tp_pessoa and d.COD_PESSOA = a.cod_pessoa " +
                                                 " Where 1 = 1 ";


            if (!string.IsNullOrEmpty(situacao))
                _count = _count + string.Format(" And b.des_nome = \'{0}\' ", situacao);
            else
                _count = _count + string.Format(" And b.cod_situacao not in ({0}) ", "2,3");


            if (!string.IsNullOrEmpty(estagio))
                _count = _count + string.Format(" And c.des_nome = \'{0}\' ", estagio);

            if (!string.IsNullOrEmpty(cliente))
                _count = _count + string.Format(" And d.DES_PESSOA = \'{0}\' or d.DES_FANTASIA = \'{1}\' ", cliente, cliente);


            int total_de_linhas = db.Database.SqlQuery<int>(_count).FirstOrDefault();



            IEnumerable<PS_Sac> filteredCompanies;
            filteredCompanies = db.PS_Sac;

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            if (!string.IsNullOrEmpty(situacao))
            {
                filteredCompanies = filteredCompanies.Where(c => c.PS_Situacao_Sac.des_nome.ToUpper().Contains(situacao.ToUpper()));
            }
            else
            {
                filteredCompanies = filteredCompanies.Where(c => !listIDs.Contains((int)c.cod_situacao));
            }


            if (!string.IsNullOrEmpty(estagio))
                filteredCompanies = filteredCompanies.Where(c => c.PS_Situacao_Sac.des_nome.ToUpper().Contains(estagio.ToUpper()));

            if (!string.IsNullOrEmpty(cliente))
                filteredCompanies = filteredCompanies.Where(c => c.PS_Pessoas_Sac.des_pessoa.ToUpper().Contains(cliente.ToUpper()) || c.PS_Pessoas_Sac.des_fantasia.ToUpper().Contains(cliente.ToUpper()));


            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new
                         {
                             sac_id = c.cod_sac,
                             des_pessoa = c.PS_Pessoas_Sac.des_pessoa,
                             des_fantasia = c.PS_Pessoas_Sac.des_fantasia,
                             des_assunto = c.des_assunto,
                             des_situacao = c.PS_Situacao_Sac.des_nome,
                             des_estagio = c.PS_Estagio_Sac.des_nome,
                             acao = ""
                         };
            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total_de_linhas,
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        



        public ActionResult Create(string tipo_pessoa = null, string cod_pessoa = null)
        {

            ViewBag.tp_pessoa = new SelectList(db.Combo.Where(p => p.TIPO == 6), "Value", "Text", string.IsNullOrEmpty(tipo_pessoa) ? "" : tipo_pessoa);
            

            if (string.IsNullOrEmpty(cod_pessoa))
            {
              //  ViewBag.cod_pessoa = new SelectList(db.PS_Pessoas_Sac.Where(p => p.tipo == "Q").ToList(), "cod_pessoa", "FullName");
                ViewBag.des_pessoa = "";
                ViewBag.cod_pessoa = "";
            }
            else
            {
               // ViewBag.cod_pessoa = new SelectList(db.PS_Pessoas_Sac.Where(p => p.tipo == tipo_pessoa && p.cod_pessoa == cod_pessoa).ToList(), "cod_pessoa", "FullName", cod_pessoa);
                ViewBag.des_pessoa = db.PS_Pessoas_Sac.Where(a => a.tipo == tipo_pessoa && a.cod_pessoa == cod_pessoa).Select(a => a.des_pessoa).First();
                ViewBag.cod_pessoa = db.PS_Pessoas_Sac.Where(a => a.tipo == tipo_pessoa && a.cod_pessoa == cod_pessoa).Select(a => a.cod_pessoa).First();
            }

            if (!string.IsNullOrEmpty(cod_pessoa))
            {
                ViewBag.existepessoa = "S";
            }

            ViewBag.cod_tipo = new SelectList(db.PS_Tipo_Sac.ToList(), "cod_tipo", "des_nome" );
            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome");
            ViewBag.cod_origem = new SelectList(db.PS_Origem_Sac.ToList(), "cod_origem", "des_nome");
            ViewBag.cod_classe = new SelectList(db.PS_Classe_Sac.ToList(), "cod_classe", "des_nome", 1);
            ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.Where(a => a.cod_classe == 1).ToList(), "cod_categoria", "des_nome", 1);
            ViewBag.cod_estagio = new SelectList(db.PS_Estagio_Sac.Where(p => p.ind_finalizacao == "N").ToList(), "cod_estagio", "des_nome");

            return View();
        }
        //


        public JsonResult GetPessoasAutocomplete(string id, string terms)
        {
            if (!string.IsNullOrEmpty(terms))
                terms = terms.ToUpper();


            var states = (from a in db.PS_Pessoas_Sac.Where(p => p.tipo == id && (p.des_fantasia.ToUpper().Contains(terms) || p.des_pessoa.ToUpper().Contains(terms))) select a);


            return Json(states);
        }

        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public JsonResult GetPessoas(string id)
        {
            var states = (from a in
                              db.PS_Pessoas_Sac.Where(p => p.tipo == id )
                          select new SelectListItem
                          {
                              Value = a.cod_pessoa,
                              Text = (string.IsNullOrEmpty(a.des_fantasia) ?
                              a.des_pessoa + " - " + a.des_fantasia + "   (" + a.cod_pessoa + ")" 
                              :
                              a.des_fantasia + " - " + a.des_pessoa + "   (" + a.cod_pessoa + ")"
                              ) 
                              
                              + ( string.IsNullOrEmpty(a.des_empresa) ? "" : "   (" + a.des_empresa + ")" ),
                              Selected = false
                          });
             


            return Json(new SelectList(states.OrderBy(p => p.Text), "Value", "Text"));
        }

        public JsonResult GetCategorias(int id)
        {

            var states = (from a in
                              db.PS_Categorias_Sac.Where(p => p.cod_classe == id)
                          select new SelectListItemCat
                          {
                              Value = a.cod_categoria,
                              Text = a.des_nome,
                              Selected = false
                          });


            return Json(new SelectList(states, "Value", "Text"));
        }

        public JsonResult GetMotivos(int id)
        {

            var states = (from a in
                              db.Tp_Procedimento_Motivos.Where(p => p.COD_TIPO == id)
                          select new SelectListItemCat
                          {
                              Value = a.MOTIVOID,
                              Text = a.DES_NOME,
                              Selected = false
                          });


            return Json(new SelectList(states, "Value", "Text"));
        }

        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        [ValidateInput(false)]
        public ActionResult Create(PS_Sac data, bool continueAdd, FormCollection form, IEnumerable<HttpPostedFileBase> files)
        {


            


            ViewBag.tp_pessoa = new SelectList(db.Combo.Where(p => p.TIPO == 6), "Value", "Text", data.tp_pessoa);
            //ViewBag.cod_pessoa = new SelectList(db.PS_Pessoas_Sac.ToList(), "cod_pessoa", "FullName", data.cod_pessoa);
            ViewBag.cod_tipo = new SelectList(db.PS_Tipo_Sac.ToList(), "cod_tipo", "des_nome", data.cod_tipo);

            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome", data.cod_situacao);
            ViewBag.cod_origem = new SelectList(db.PS_Origem_Sac.ToList(), "cod_origem", "des_nome", data.cod_origem);
            ViewBag.cod_classe = new SelectList(db.PS_Classe_Sac.ToList(), "cod_classe", "des_nome", data.cod_classe);
            ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.Where(a => a.cod_classe == data.cod_classe).ToList(), "cod_categoria", "des_nome", data.cod_categoria);
            ViewBag.cod_estagio = new SelectList(db.PS_Estagio_Sac.Where(p => p.ind_finalizacao == "N").ToList(), "cod_estagio", "des_nome", data.cod_estagio);

            if (string.IsNullOrEmpty(data.cod_pessoa))
            {
                ViewBag.cod_pessoa = new SelectList(db.PS_Pessoas_Sac.Where(p => p.tipo == "Q").ToList(), "cod_pessoa", "FullName");
            }
            else
            {
                ViewBag.cod_pessoa = new SelectList(db.PS_Pessoas_Sac.Where(p => p.tipo == data.tp_pessoa && p.cod_pessoa == data.cod_pessoa).ToList(), "cod_pessoa", "FullName", data.cod_pessoa);
            }




            if (string.IsNullOrEmpty(data.tp_pessoa))
            {
                ModelState.AddModelError("tp_pessoa", "Tipo deve ser informado");
                return View(data);
            }


            if (string.IsNullOrEmpty(data.cod_pessoa))
            {
                ModelState.AddModelError("cod_pessoa", "Pessoa deve ser informada");
                return View(data);
            }


            var PS_Pessoas_Sac = db.PS_Pessoas_Sac.Where(a => a.tipo == data.tp_pessoa && a.cod_pessoa == data.cod_pessoa).FirstOrDefault();
            if (PS_Pessoas_Sac == null)
            {
                ModelState.AddModelError("cod_pessoa", "Pessoa não foi localizada, por favor verifique.");
                return View(data);
            }


            ModelState.Clear();
            data.cod_sac = db.Database.SqlQuery<Int32>("select PS_Sac_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            data.dta_abertura = System.DateTime.Now;
            data.cod_usuario = cd_usuario;
            TryValidateModel(data);

            if (files != null)
            {
                foreach (var a in files)
                {
                    if (a != null)
                    {
                        if (a.ContentLength > 0)
                        {

                            int tamanho = (int)a.InputStream.Length;
                            string contentype = a.ContentType;
                            byte[] arq = new byte[tamanho];

                            a.InputStream.Read(arq, 0, tamanho);
                            byte[] arqUp = arq;


                            int sacarquivoid = db.Database.SqlQuery<Int32>("select SacArquivoSeq.NextVal from dual ").FirstOrDefault<Int32>();


                            string NomeArquivo = a.FileName;
                            var path = Path.Combine(Server.MapPath(Settings.caminho_arquivos_procedimento), NomeArquivo);

                            SacArquivo doc = new SacArquivo
                            {
                                id = sacarquivoid,
                                sacid = data.cod_sac,
                                des_contenttype = contentype,
                                des_imagem = arqUp,
                                nome_arquivo = NomeArquivo
                            };
                            db.SacArquivo.Add(doc);
                            //a.SaveAs(path);
                        }
                    }
                }
            }



            if (ModelState.IsValid)
            {
                sac_troca_estagio nova_linha = AddNovaLinhaTrocaEstagio(data.cod_sac, null, data.cod_estagio, data.des_descricao);
                
                db.sac_troca_estagio.Add(nova_linha);
                db.PS_Sac.Add(data);
                db.SaveChanges();

                string email_logad = db.Usuario.Where(a => a.CD_USUARIO == cd_usuario).Select(a => a.EMAIL).FirstOrDefault();
                string nome_logado = db.Usuario.Where(a => a.CD_USUARIO == cd_usuario).Select(a => a.NOME).FirstOrDefault();

                _email.EnviarEmailSac(data.cod_sac, "BasicSacInformation.htm", nome_logado, email_logad);

                return continueAdd ? RedirectToAction("Create") : RedirectToAction("List");
                
            }
            return View(data);
        }

        
        //public ActionResult ReabreSac(int id)
        //{
        //    var data = new PSSacPSSacModel()
        //    {
        //        SacOriginal = _sacNewDataServices.Find(id),
        //        SacNovo = new PS_Sac() { dta_abertura = System.DateTime.Now },
        //        Sacs = new Ps_Sac_Ps_Sac()
        //    };

        //    return View(data);
        //}

        [HttpPost]
        public JsonResult Reabrir(int cod_sac, string obs, int cod_estagio_reabre)
        {
            PS_Sac data = db.PS_Sac.Find(cod_sac);
            int cod_sac_novo = db.Database.SqlQuery<Int32>("select PS_Sac_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            PS_Sac dataNovo = new PS_Sac()
            {
                cod_categoria = data.cod_categoria,
                cod_classe = data.cod_classe,
                cod_estagio = cod_estagio_reabre,
                cod_origem = data.cod_origem,
                cod_pessoa = data.cod_pessoa,
                cod_sac = cod_sac_novo,
                cod_situacao = data.cod_situacao,
                cod_tipo = data.cod_tipo,
                cod_usuario = data.cod_usuario,
                des_assunto = data.des_assunto,
                des_descricao = data.des_descricao,
                des_nome = data.des_nome,
                des_solucao = data.des_solucao,
                dta_abertura = System.DateTime.Now,
                dta_finalizacao = null,
                tp_pessoa = data.tp_pessoa
                
            };






            //data.cod_estagio = cod_estagio_reabre;
            //data.cod_situacao = 1;
            //data.dta_finalizacao = null;
            //db.Entry(data).State = EntityState.Modified;


            Ps_Sac_Ps_Sac newSac = new Ps_Sac_Ps_Sac()
            {
                rec = db.Database.SqlQuery<Int32>("select Ps_Sac_Ps_Sac_Seq.NextVal from dual ").FirstOrDefault<Int32>(),
                SacOriginal = data,
                SacNovo = dataNovo,
                //cod_sac_original = cod_sac,
                //cod_sac_novo = cod_sac_novo,
                obs = obs,
                cod_usuario = cd_usuario
            };

            Ps_Sac_Reabertura new_data = new Ps_Sac_Reabertura()
            {
                id = db.Database.SqlQuery<Int32>("select Ps_Sac_ReaberturaSeq.NextVal from dual ").FirstOrDefault<Int32>(),
                cod_sac = cod_sac,
                //PS_Sac = data,
                cod_usuario = cd_usuario,
                dta_reabertura = System.DateTime.Now,
                obs = obs
            };




            //var Sacgarantia = db.SacGarantia.Where(a => a.cod_sac == cod_sac).FirstOrDefault();
            //var SacProcedimento = db.SacProcedimento.Where(a => a.cod_sac == cod_sac).FirstOrDefault();

            //if (Sacgarantia != null) { db.SacGarantia.Remove(Sacgarantia); }
            //if (SacProcedimento != null) { db.SacProcedimento.Remove(SacProcedimento); }

            //sac_troca_estagio nova_linha = AddNovaLinhaTrocaEstagio(data.cod_sac, null, data.cod_estagio, obs);
            sac_troca_estagio nova_linha = AddNovaLinhaTrocaEstagio(cod_sac_novo, null, cod_estagio_reabre, obs);

            db.sac_troca_estagio.Add(nova_linha);
            db.Ps_Sac_Reabertura.Add(new_data);
            db.Ps_Sac_Ps_Sac.Add(newSac);
            db.PS_Sac.Add(dataNovo);


            try
            {
                db.SaveChanges();
                //_sacps_sac_reaberturaServices.Add(new_data);
                //_sacSacDataServices.Add(newSac);

                _email.EnviarEmailSac(cod_sac, "BasicSacInformation.htm");
                return Json(new { resultado = "OK", newCodSac = cod_sac_novo });

            }
            catch (Exception E)
            {
                return Json(new { resultado = "NOK", MErro = E.Message });
            }

            
        }

      
        //
        // GET: /TipoLead/Edit/5
        public ActionResult Edit(int id)
        {
            PS_Sac data = db.PS_Sac.Find(id);
            bool _ind_finalizada = data.PS_Estagio_Sac != null ? data.PS_Estagio_Sac.ind_finalizacao == "S" : false;

            if (_ind_finalizada)
            { return RedirectToAction("Details", new { id = id }); }


            ViewBag.cod_empresa = data.GetEmpresa(data.cod_empresa);
            

            ViewBag.tp_pessoa = new SelectList(db.Combo.Where(p => p.TIPO == 6), "Value", "Text", data.tp_pessoa);
            ViewBag.cod_pessoa = (from a in
                                      db.PS_Pessoas_Sac
                                  select new SelectListItem
                                  {
                                      Value = a.cod_pessoa,
                                      Text = string.Concat(a.tipo, ": ", a.des_pessoa),
                                      Selected = data.tp_pessoa == a.tipo && data.cod_pessoa == a.cod_pessoa
                                  });
            ViewBag.cod_tipo = new SelectList(db.PS_Tipo_Sac.ToList(), "cod_tipo", "des_nome", data.cod_tipo);
            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome", data.cod_situacao);
            ViewBag.cod_origem = new SelectList(db.PS_Origem_Sac.ToList(), "cod_origem", "des_nome", data.cod_origem);
            ViewBag.cod_classe = new SelectList(db.PS_Classe_Sac.ToList(), "cod_classe", "des_nome", data.cod_classe);
            ViewBag.cod_estagio = new SelectList(db.PS_Estagio_Sac.ToList(), "cod_estagio", "des_nome", data.cod_estagio);
            ViewBag.bloqueiaEstagio = "sim";

            int? cod_classe = data.cod_classe;
            if (cod_classe.HasValue)
            {
                ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.Where(a => a.cod_classe == cod_classe).ToList(), "cod_categoria", "des_nome", data.cod_categoria);
            }
            else
            {
                ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.ToList(), "cod_categoria", "des_nome", data.cod_categoria);

            }


            if (data == null)
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
        public ActionResult Edit(PS_Sac data, bool continueAdd, bool isDelete)
        {
            bool _ind_finalizada = data.PS_Estagio_Sac != null ? data.PS_Estagio_Sac.ind_finalizacao == "S" : false;

            if (_ind_finalizada)
            { return RedirectToAction("Details", new { id = data.cod_sac }); }

            ViewBag.tp_pessoa = new SelectList(db.Combo.Where(p => p.TIPO == 6), "Value", "Text", data.tp_pessoa);
            ViewBag.cod_pessoa = new SelectList(db.PS_Pessoas_Sac.ToList(), "cod_pessoa", "FullName", data.cod_pessoa);
            ViewBag.cod_tipo = new SelectList(db.PS_Tipo_Sac.ToList(), "cod_tipo", "des_nome", data.cod_tipo);
            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome", data.cod_situacao);
            ViewBag.cod_origem = new SelectList(db.PS_Origem_Sac.ToList(), "cod_origem", "des_nome", data.cod_origem);
            ViewBag.cod_classe = new SelectList(db.PS_Classe_Sac.ToList(), "cod_classe", "des_nome", data.cod_classe);
            ViewBag.bloqueiaEstagio = "sim";
            int? cod_classe = data.cod_classe;
            if (cod_classe.HasValue)
            {
                ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.Where(a => a.cod_classe == cod_classe).ToList(), "cod_categoria", "des_nome", data.cod_categoria);
            }
            else
            {
                ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.ToList(), "cod_categoria", "des_nome", data.cod_categoria);
            }
            ViewBag.cod_estagio = new SelectList(db.PS_Estagio_Sac.ToList(), "cod_estagio", "des_nome", data.cod_estagio);




            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    _email.EnviarEmailSac(data.cod_sac, "BasicSacInformation.htm");
                    return continueAdd ? RedirectToAction("Edit", new { id = data.cod_sac }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    PS_Sac dataDelete = db.PS_Sac.Find(data.cod_sac);
                    db.PS_Sac.Remove(dataDelete);
                    db.SaveChanges();
                    _email.EnviarEmailSac(data.cod_sac, "BasicSacInformation.htm");
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
                    return RedirectToAction("Edit", new { id = data.cod_sac });
                }
                return RedirectToAction("List");
            }
        }



        public PartialViewResult GetDocs(int id)
        {

            var Itens = db.SacArquivo.Where(a => a.sacid == id).ToList();
            ViewBag.GarantiaId = id;

            return PartialView(Itens);
        }



        public ActionResult Download(int id)
        {
            var docs = (from a in db.SacArquivo
                        where a.id == id
                        select new
                        {
                            doc = a.des_imagem,
                            tipo = a.des_contenttype
                        }).ToList();

            byte[] imagedata = (byte[])docs[0].doc;

            return File(imagedata, docs[0].tipo);



        }




        [HttpPost]
        public ActionResult RemoveDocFromSac(int id)
        {
            // Remove the item from the cart
            SacArquivo varDelete = db.SacArquivo.Where(p => p.id == id).Single();
            db.SacArquivo.Remove(varDelete);

            try
            {
                db.SaveChanges();
                return Json(new { id = id, Msg = "Ok, Atualizado item com suceso ", hasError = false });
            }
            catch (Exception e)
            {
                return Json(new { id = id, Msg = "Erro ao excluir", hasError = true });
            }


        }




        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDocSac(int sacid)
        {
            for (int i = 0; i <= Request.Files.Count - 1; i++)
            {
                int tamanho = (int)Request.Files[i].InputStream.Length;
                string contentype = Request.Files[i].ContentType;
                byte[] arq = new byte[tamanho];

                Request.Files[i].InputStream.Read(arq, 0, tamanho);
                byte[] arqUp = arq;


                int nextt = db.Database.SqlQuery<Int32>("select SacArquivoSeq.NextVal from dual ").FirstOrDefault<Int32>();
                string NomeArquivo = sacid.ToString() + '_' + nextt.ToString() + '_' + string.Format("{0}", Path.GetFileName(Request.Files[i].FileName));

                SacArquivo cat = new SacArquivo
                {
                    id = nextt,
                    sacid = sacid,
                    caminho = NomeArquivo,
                    des_contenttype = contentype,
                    des_imagem = arqUp,
                    nome_arquivo = Request.Files[i].FileName
                };

                db.SacArquivo.Add(cat);
            }


            try
            {
                db.SaveChanges();

            }
            catch
            {
                return InvokeHttpNotFound();
            }

            return RedirectToAction("GetDocs", new { id = sacid });
        }





        public ActionResult Details(int id)
        {
            string ids = id.ToString();
            ViewBag.comentarios = db.ListaComentarios.Where(a => a.cod_interno == ids && a.tipo_nota == "SAC").ToList();
            ViewBag.troca_estagio = db.sac_troca_estagio.Where(a => a.cod_sac == id).OrderBy(p => p.num_seq).ToList();

            PS_Sac data = db.PS_Sac.Find(id);

            ViewBag.tp_pessoa = new SelectList(db.Combo.Where(p => p.TIPO == 6), "Value", "Text", data.tp_pessoa);
            ViewBag.cod_pessoa = (from a in
                                      db.PS_Pessoas_Sac
                                  select new SelectListItem
                                  {
                                      Value = a.cod_pessoa,
                                      Text = string.Concat(a.tipo, ": ", a.des_pessoa),
                                      Selected = data.tp_pessoa == a.tipo && data.cod_pessoa == a.cod_pessoa
                                  });
            ViewBag.cod_tipo = new SelectList(db.PS_Tipo_Sac.ToList(), "cod_tipo", "des_nome", data.cod_tipo);
            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome", data.cod_situacao);
            ViewBag.cod_origem = new SelectList(db.PS_Origem_Sac.ToList(), "cod_origem", "des_nome", data.cod_origem);
            ViewBag.cod_classe = new SelectList(db.PS_Classe_Sac.ToList(), "cod_classe", "des_nome", data.cod_classe);
            ViewBag.cod_estagio = new SelectList(db.PS_Estagio_Sac.ToList(), "cod_estagio", "des_nome", data.cod_estagio);
            ViewBag.cod_estagio_reabre = new SelectList(db.PS_Estagio_Sac.ToList(), "cod_estagio", "des_nome");
            int? cod_classe = data.cod_classe;
            if (cod_classe.HasValue)
            {
                ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.Where(a => a.cod_classe == cod_classe).ToList(), "cod_categoria", "des_nome", data.cod_categoria);
            }
            else
            {
                ViewBag.cod_categoria = new SelectList(db.PS_Categorias_Sac.ToList(), "cod_categoria", "des_nome", data.cod_categoria);

            }


            if (data == null)
            {
                return InvokeHttpNotFound();
            }



            ViewBag.cod_gerado = RetornaDocGerado(data.cod_sac);
            var returndata = new PSSacModel
            {
                Ps_Sac = data,
                cod_novo = db.Ps_Sac_Ps_Sac.Where(a => a.cod_sac_original == data.cod_sac).Select(a => a.cod_sac_novo).DefaultIfEmpty(0).First(),
                cod_original = db.Ps_Sac_Ps_Sac.Where(a => a.cod_sac_novo == data.cod_sac).Select(a => a.cod_sac_original).DefaultIfEmpty(0).First()
            };

            return View(returndata);
        }

        public int RetornaDocGerado(int cod_sac)
        {
            int retorno = 0;


            SacProcedimento procedimento = db.SacProcedimento.Where(p => p.cod_sac == cod_sac).FirstOrDefault();
            if (procedimento != null)
            {
                retorno = procedimento.cod_procedimento;
            }


            SacGarantia garantia = db.SacGarantia.Where(p => p.cod_sac == cod_sac).FirstOrDefault();
            if (garantia != null)
            {
                retorno = garantia.garantiaId;
            }

            //int doc = procedimento.cod_procedimento == null ? 0 : procedimento.cod_procedimento;

            return retorno;
        }




        public ActionResult Resposta(int id)
        {
            Ps_Sac_Resposta_Model data = new Ps_Sac_Resposta_Model { Obs = "", PS_Sac = db.PS_Sac.Find(id), motivo = db.PS_Sac.Where(a => a.cod_sac == id).Select(a => a.des_assunto).FirstOrDefault() };
            bool _ind_finalizada = data.PS_Sac.PS_Estagio_Sac != null ? data.PS_Sac.PS_Estagio_Sac.ind_finalizacao == "S" : false;

            if (_ind_finalizada)
            { return RedirectToAction("Details", new { id = id }); }


            if (data.PS_Sac == null)
            {
                return InvokeHttpNotFound();
            }

            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.Where(a => a.cod_situacao != 2).ToList(), "cod_situacao", "des_nome", data.PS_Sac.cod_situacao);
            ViewBag.cod_estagio = new SelectList(db.PS_Estagio_Sac.ToList(), "cod_estagio", "des_nome", data.PS_Sac.cod_estagio);
            ViewBag.troca_estagio = db.sac_troca_estagio.Where(a => a.cod_sac == id).OrderBy(p => p.num_seq).ToList();
            return View(data);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ValidateInput(false)]
        public ActionResult Resposta(Ps_Sac_Resposta_Model data, int? cod_estagio, int? cod_situacao, bool continueAdd)
        {
            bool _ind_finalizada = data.PS_Sac.PS_Estagio_Sac != null ? data.PS_Sac.PS_Estagio_Sac.ind_finalizacao == "S" : false;
            if (_ind_finalizada) { return RedirectToAction("Details", new { id = data.PS_Sac.cod_sac }); }


            PS_Sac data_update = db.PS_Sac.Find(data.PS_Sac.cod_sac);
            if (data_update == null) { return View(data); }

            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome", data.PS_Sac.cod_situacao);
            ViewBag.cod_estagio = new SelectList(db.PS_Estagio_Sac.ToList(), "cod_estagio", "des_nome", data.PS_Sac.cod_estagio);
            ViewBag.troca_estagio = db.sac_troca_estagio.Where(a => a.cod_sac == data.PS_Sac.cod_sac).OrderBy(p => p.num_seq).ToList();



            if (ModelState.IsValid)
            {
                int? cod_estagio_atual = data_update.cod_estagio;

                int? cod_situacao_atual = data_update.cod_situacao;
                string indica = db.PS_Estagio_Sac.Where(a => a.cod_estagio == cod_estagio).Select(a => a.ind_finalizacao).FirstOrDefault();
                bool indica_estagio_final = indica == "S";



                if (indica_estagio_final)
                {
                    data_update.dta_finalizacao = System.DateTime.Now;
                    data_update.des_solucao = data.Obs;
                    return FinalizaSac(data_update.cod_sac, (int)cod_estagio, (int)cod_situacao, data.Obs);
                }
                else
                {

                    sac_troca_estagio altera_linha = AddDataSaidaEstagioAnterior(data_update.cod_sac);
                    sac_troca_estagio nova_linha = AddNovaLinhaTrocaEstagio(data_update.cod_sac, data_update.cod_estagio, cod_estagio, data.Obs);


                    data_update.cod_estagio = cod_estagio.HasValue ? cod_estagio : data_update.cod_estagio;
                    data_update.cod_situacao = cod_situacao.HasValue ? cod_situacao : data_update.cod_situacao;
                    data_update.des_assunto = data.motivo;


                    db.Entry(data_update).State = EntityState.Modified;
                    db.Entry(altera_linha).State = EntityState.Modified;
                    db.sac_troca_estagio.Add(nova_linha);

                    db.SaveChanges();
                    _email.EnviarEmailSac(data_update.cod_sac, "BasicSacInformation.htm");
                    return continueAdd ? RedirectToAction("Resposta", new { id = data_update.cod_sac }) : RedirectToAction("Details", new { id = data_update.cod_sac });
                }
            }

            return View(data);
        }




        private sac_troca_estagio AddNovaLinhaTrocaEstagio(int cod_sac, int? cod_estagio_anterior, int? cod_estagio_novo, string msg)
        {

            return new sac_troca_estagio
            {
                cod_estagio_entrada = cod_estagio_novo,
                cod_estagio_anterior = cod_estagio_anterior,
                cod_usuario = cd_usuario,
                cod_sac = cod_sac,
                num_seq = db.Database.SqlQuery<Int32>("select sac_troca_estagio_seq.NextVal from dual ").FirstOrDefault<Int32>(),
                dta_entrada = System.DateTime.Now,
                dta_saida = null,
                obs = msg
            };
        }
        private sac_troca_estagio AddDataSaidaEstagioAnterior(int cod_sac)
        {
            int? num_seq = db.sac_troca_estagio.Where(a => a.cod_sac == cod_sac && a.dta_saida == null).Max(p => p.num_seq);
            sac_troca_estagio sac_troca_update = db.sac_troca_estagio.Find(num_seq, cod_sac);
            sac_troca_update.dta_saida = System.DateTime.Now;
            return sac_troca_update;
        }


        public ActionResult FinalizaSac(int cod_sac, int cod_novo_estagio, int cod_nova_situacao, string msg)
        {
            PS_Sac data = db.PS_Sac.Find(cod_sac);
            int cod_situacao_finalizado = int.Parse(Settings.cod_situacao_finalizacao);
            int cod_situacao_cancelado = int.Parse(Settings.cod_situacao_cancelada);
            int cod_situacao_atendimento = int.Parse(Settings.cod_situacao_ematendimento);
            string tp_finalizacao = data.PS_Tipo_Sac != null ? data.PS_Tipo_Sac.tp_finalizacao : "F";
            bool ind_cancelamento = cod_nova_situacao == cod_situacao_cancelado;

            if (ind_cancelamento) { tp_finalizacao = "F"; cod_situacao_finalizado = cod_situacao_cancelado; }
            string[] em_atendimento = new string[] { "G", "P" };
            string[] situaceos_finais = new string[] { "F", "G", "P" };

            if (em_atendimento.Contains(tp_finalizacao)) { cod_situacao_finalizado = cod_situacao_atendimento; }



            // coloca a situacao final + solucao final
            data.cod_situacao = cod_situacao_finalizado;
            data.cod_estagio = cod_novo_estagio;
            data.des_solucao = String.IsNullOrEmpty(msg) ? data.des_solucao : msg;
            data.dta_finalizacao = System.DateTime.Now;

            //adiciona a linha finalizadopra nas trocas
            sac_troca_estagio finaliza_ultima_troca = AddDataSaidaEstagioAnterior(data.cod_sac);
            sac_troca_estagio addsoluction = AddNovaLinhaTrocaEstagio(data.cod_sac,data.cod_situacao, data.cod_situacao, data.des_solucao);
            addsoluction.dta_saida = System.DateTime.Now;
            db.sac_troca_estagio.Add(addsoluction);


            if (tp_finalizacao.Equals("F") || ind_cancelamento)
            {
                db.Entry(data).State = EntityState.Modified;
                db.Entry(finaliza_ultima_troca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = data.cod_sac });
            }



            //FINALZA + PROCEDIMETNO
            if (tp_finalizacao.Equals("P"))
            {
                Session["PS_Sac"] = data;
                return RedirectToAction("CriarProcedimento", new { id = data.cod_sac });
            }

            if (tp_finalizacao.Equals("G"))
            {
                Session["PS_Sac"] = data;
                return RedirectToAction("CriarGarantia", new { id = data.cod_sac });
            }

            return RedirectToAction("Resposta", new { id = data.cod_sac });

        }

        //public ActionResult FinalizaSac(int cod_sac, int cod_estagio, int cod_situacao, string msg)
        //{
        //    if (!cod_estagio.HasValue) return Redirect;

        //    int cod_finalizado = int.Parse(Settings.cod_situacao_finalizacao);
        //    int cod_cancelado = int.Parse(Settings.cod_situacao_cancelada);
        //    int cod_atendimento = int.Parse(Settings.cod_situacao_ematendimento);
        //    PS_Estagio_Sac estagio = db.PS_Estagio_Sac.Find(cod_estagio);
        //    PS_Sac data = db.PS_Sac.Find(cod_sac);
        //    string tp_finalizacao = data.PS_Tipo_Sac != null ? data.PS_Tipo_Sac.tp_finalizacao : "F";
        //    bool ind_calamento;
        //    if (cod_cancelado == cod_situacao) { ind_calamento = true; tp_finalizacao = "F"; cod_finalizado = cod_cancelado; } else { ind_calamento = false; }



        //    if (estagio.ind_finalizacao == "S" || ind_calamento)
        //    {

        //        string[] em_atendimento = new string[] { "G", "P" };
        //        if (em_atendimento.Contains(tp_finalizacao))
        //        {
        //            cod_finalizado = cod_atendimento;
        //        }


        //        if (tp_finalizacao.Equals("F", StringComparison.InvariantCultureIgnoreCase) || ind_calamento)
        //        {
        //            // coloca a situacao final + solucao final
        //            data.cod_situacao = cod_finalizado;
        //            data.des_solucao = String.IsNullOrEmpty(msg) ? data.des_solucao : msg;
        //            data.dta_finalizacao = System.DateTime.Now;
        //            db.Entry(data).State = EntityState.Modified;


        //            // altera a data saida da linha de troca
        //            int num_seq = db.sac_troca_estagio.Where(a => a.cod_sac == data.cod_sac && a.dta_saida == null).Max(p => p.num_seq);
        //            sac_troca_estagio sac_troca_update = db.sac_troca_estagio.Find(num_seq, data.cod_sac);
        //            sac_troca_update.dta_saida = System.DateTime.Now;
        //            db.Entry(sac_troca_update).State = EntityState.Modified;

        //            db.SaveChanges();
        //        }

        //        //FINALZA + PROCEDIMETNO
        //        if (tp_finalizacao.Equals("P", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //         //   Session["PS_Sac"] = data;
        //            Response.red ("CriarProcedimento", new { data = data });
        //        }





        //    }

        //}

        public ActionResult CriarGarantia(int id)
        {
            PS_Sac sac = (PS_Sac)Session["PS_Sac"];
            if (sac == null) { RedirectToAction("Resposta", new { id = id }); }

            string _nome = db.PS_Sac.Where(a => a.cod_sac == id).Select(a => a.PS_Pessoas_Sac != null ? a.PS_Pessoas_Sac.des_pessoa : a.des_nome != null ? a.des_nome : "Não atribuído").FirstOrDefault();
            string _origem = db.PS_Origem_Sac.Where(a => a.cod_origem == sac.cod_origem).Select(a => a.des_nome).FirstOrDefault();
            string _tipo = db.PS_Tipo_Sac.Where(a => a.cod_tipo == sac.cod_tipo).Select(a => a.des_nome).FirstOrDefault();

            
            var data_pass = new Ps_Sac_Garantia_Model
            {
                Garantia = new Garantia(),
                cod_sac = sac.cod_sac,
                Nome_Sac = _nome,
                des_assunto = sac.des_assunto,
                origem = _origem,
                tipo = _tipo,
                solicitacao = sac.des_assunto,
                _total_aberto = sac._total_aberto
            };

            int _cod_pessoa = 0;
            int _cod_representante = 0;
            string _nomeRepresentatne = "";


            try { _cod_pessoa = Convert.ToInt32(sac.cod_pessoa); } catch { _cod_pessoa = 0; }
            try { _cod_representante = db.Clientes.Where(A => A.CD_CADASTRO == _cod_pessoa).Select(p => p.CD_REPRESENTANTE).First(); } catch { _cod_representante = 0; }
            try { _nomeRepresentatne = db.Clientes.Where(A => A.CD_CADASTRO == _cod_representante).Select(p => p.RAZAO_REPRES).First(); } catch { _nomeRepresentatne = ""; }
            data_pass.cod_cliente = _cod_pessoa;
            data_pass.cod_representante = _cod_representante;
            data_pass.NomeRepresentante = _nomeRepresentatne;
            ViewBag.cod_cliente = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == _cod_pessoa).ToList(), "CD_CADASTRO", "RAZAO", _cod_pessoa);


            return View(data_pass);
        }


        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult CriarGarantia(Ps_Sac_Garantia_Model data, bool continueAdd, FormCollection form)
        {
            #region initicalize
                    PS_Sac sac = (PS_Sac)Session["PS_Sac"];
                    if (sac == null) { RedirectToAction("Resposta", new { id = data.cod_sac }); }

                    int id = data.cod_sac;

                    string _nome = db.PS_Sac.Where(a => a.cod_sac == id).Select(a => a.PS_Pessoas_Sac != null ? a.PS_Pessoas_Sac.des_pessoa : a.des_nome != null ? a.des_nome : "Não atribuído").FirstOrDefault();
                    string _origem = db.PS_Origem_Sac.Where(a => a.cod_origem == sac.cod_origem).Select(a => a.des_nome).FirstOrDefault();
                    string _tipo = db.PS_Tipo_Sac.Where(a => a.cod_tipo == sac.cod_tipo).Select(a => a.des_nome).FirstOrDefault();


                    var data_pass = new Ps_Sac_Garantia_Model
                    {
                        Garantia = data.Garantia,
                        cod_sac = data.cod_sac,
                        Nome_Sac = _nome,
                        des_assunto = sac.des_assunto,
                        origem = _origem,
                        tipo = _tipo,
                        solicitacao = sac.des_assunto,
                        _total_aberto = sac._total_aberto
                    };

                    int _cod_pessoa = 0;
                    int _cod_representante = 0;
                    string _nomeRepresentatne = "";


                    try { _cod_pessoa = Convert.ToInt32(sac.cod_pessoa); } catch { _cod_pessoa = 0; }
                    try { _cod_representante = db.Clientes.Where(A => A.CD_CADASTRO == _cod_pessoa).Select(p => p.CD_REPRESENTANTE).First(); } catch { _cod_representante = 0; }
                    try { _nomeRepresentatne = db.Clientes.Where(A => A.CD_CADASTRO == _cod_representante).Select(p => p.RAZAO_REPRES).First(); } catch { _nomeRepresentatne = ""; }
                    data_pass.cod_cliente = _cod_pessoa;
                    data_pass.cod_cliente = _cod_representante;
                    data_pass.NomeRepresentante = _nomeRepresentatne;
                    ViewBag.cod_cliente = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == _cod_pessoa).ToList(), "CD_CADASTRO", "RAZAO", _cod_pessoa);

            #endregion


            #region logic_to_add_garantia
                    int NextGarantia = db.Database.SqlQuery<Int32>("select GarantiaSeq.NextVal from dual ").FirstOrDefault<Int32>();

                    Garantia gat = new Garantia()
                    {
                        cod_cliente = data.cod_cliente,
                        cod_representante = data.cod_representante,
                        cod_status = 1,
                        cod_transportador = 0,
                        cod_usuario = cd_usuario,
                        dta_coleta = null,
                        dta_finalizacao = null,
                        dta_inclusao = System.DateTime.Now,
                        dta_previsao_coleta = null,
                        especies = "",
                        garantiaid = NextGarantia,
                        ind_cancelada = "N",
                        ind_coleta_direta = "N",
                        ind_emitido_coleta = "N",
                        ind_emitido_nf = "N",
                        num_coleta = "",
                        num_nf_cliente = "",
                        obs = data.Garantia.obs,
                        obscoleta = "",
                        protocolo = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + NextGarantia.ToString(),
                        vlr_garantia = 0,
                        vlr_ngarantia = 0,
                        volumes = "",
                        cod_procedimento_final = 0,
                        cod_procedimento_vinculado = 0
                    };
            #endregion



            if (ModelState.IsValid)
            {
                try
                {
                    db.Garantia.Add(gat);



                    PS_Sac sac_to_update = db.PS_Sac.Find(sac.cod_sac);


                    sac_to_update.cod_estagio = sac.cod_estagio;
                    sac_to_update.cod_situacao = sac.cod_situacao;
                    sac_to_update.des_assunto = sac.des_solucao;
                    sac_to_update.dta_finalizacao = sac.dta_finalizacao;

                    sac_troca_estagio finaliza_ultima_troca = AddDataSaidaEstagioAnterior(sac.cod_sac);
                    db.Entry(sac_to_update).State = EntityState.Modified;
                    db.Entry(finaliza_ultima_troca).State = EntityState.Modified;

                    SacGarantia sacgarantia = new SacGarantia { cod_sac = sac_to_update.cod_sac, garantiaId = gat.garantiaid };
                    db.SacGarantia.Add(sacgarantia);

                    Garantia_Troca_Status nova_linha = AddNovaLinhaTrocaEstagioGat(gat.garantiaid, null, gat.cod_status, gat.obs);
                    db.Garantia_Troca_Status.Add(nova_linha);


                    db.SaveChanges();

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
                    return View(data);
                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        ModelState.AddModelError("", error.Exception.Message);
                    }
                }
                return View(data);

            }

            
            return RedirectToAction("AddItem", "GarantiaItem", new { id = gat.garantiaid, cod_cliente = _cod_pessoa });



        }

        private Garantia_Troca_Status AddNovaLinhaTrocaEstagioGat(int Garantiaid, int? cod_status_anterior, int? cod_status_novo, string obs)
        {
            return new Garantia_Troca_Status
            {
                cod_status_entrada = cod_status_novo,
                cod_status_anterior = cod_status_anterior,
                dta_troca = System.DateTime.Now,
                garantiaId = Garantiaid,
                cod_usuario = cd_usuario,
                num_seq = db.Database.SqlQuery<Int32>("select Garantia_Troca_Status_Seq.NextVal from dual ").FirstOrDefault<Int32>(),
                dta_entrada = System.DateTime.Now,
                dta_saida = null,
                obs = obs
            };
        }

        public ActionResult CriarProcedimento(int id)
        {
            PS_Sac sac = (PS_Sac)Session["PS_Sac"];
            if (sac == null) { RedirectToAction("Resposta", new { id = id }); }

            string _nome = db.PS_Sac.Where(a => a.cod_sac == id).Select(a => a.PS_Pessoas_Sac != null ? a.PS_Pessoas_Sac.des_pessoa : a.des_nome != null ? a.des_nome : "Não atribuído").FirstOrDefault();
            string _origem = db.PS_Origem_Sac.Where(a => a.cod_origem == sac.cod_origem).Select(a => a.des_nome).FirstOrDefault();
            string _tipo = db.PS_Tipo_Sac.Where(a => a.cod_tipo == sac.cod_tipo).Select(a => a.des_nome).FirstOrDefault();


            var data_pass = new Ps_Sac_Procedimento_Model
            {
                ProcedimentoAdm = new ProcedimentoAdm(),
                cod_sac = sac.cod_sac,
                Nome_Sac = _nome,
                des_assunto = sac.des_assunto,
                origem = _origem,
                tipo = _tipo,
                solicitacao = sac.des_assunto,
                _total_aberto = sac._total_aberto


            };
            //sac.PS_Pessoas_Sac != null ? sac.PS_Pessoas_Sac.des_pessoa : sac.des_nome != null ? sac.des_nome : "Não atribuído"
            string _cod_pessoa = sac.cod_pessoa.ToString();

            if (sac.tp_pessoa == "C")
            {
                _cod_pessoa = db.Contatos.Where(p => p.cod_contato == Convert.ToInt32(_cod_pessoa)).Select(a => a.DadosDoCrm.cod_pessoa).ToString();
            }
            else if (sac.tp_pessoa == "E")
            {
                _cod_pessoa = sac.cod_pessoa.ToString();
            }
            else if (sac.tp_pessoa == "L")
            {
                _cod_pessoa = null;
            }
            int teste = Convert.ToInt32(_cod_pessoa);
            
            ViewBag.cd_cadastro = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == teste).ToList(), "CD_CADASTRO", "RAZAO", _cod_pessoa);
            ViewBag.cd_tipo = new SelectList(db.TP_PROCEDIMENTO.Where(a => a.ATIVO == "S").ToList(), "CD_TIPO", "DES_TIPO", string.Empty);
            ViewBag.cd_departamento = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S").ToList(), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", string.Empty);
            ViewBag.cod_transportador = new SelectList(db.TRANSPORTADOR.ToList(), "COD_TRANSPORTADOR", "FullName", string.Empty);
            ViewBag.tp_pessoa = new SelectList(db.Combo.Where(p => p.TIPO == 6), "Value", "Text");
            ViewBag.cod_tipo = new SelectList(db.PS_Tipo_Sac.ToList(), "cod_tipo", "des_nome");
            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome");
            ViewBag.cod_origem = new SelectList(db.PS_Origem_Sac.ToList(), "cod_origem", "des_nome");
            ViewBag.cod_classe = new SelectList(db.PS_Classe_Sac.ToList(), "cod_classe", "des_nome");
            ViewBag.motivoid = new SelectList(db.Tp_Procedimento_Motivos.Where(a => a.COD_TIPO ==0).ToList(), "cod_tipo", "des_nome");




            return View(data_pass);
        }



        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult CriarProcedimento(Ps_Sac_Procedimento_Model data, bool continueAdd,
            FormCollection form, IEnumerable<HttpPostedFileBase> files)
        {
            #region initializer

            PS_Sac sac = (PS_Sac)Session["PS_Sac"];
            if (sac == null) { RedirectToAction("Resposta", new { id = sac.cod_sac }); }

            string _nome = db.PS_Sac.Where(a => a.cod_sac == sac.cod_sac).Select(a => a.PS_Pessoas_Sac != null ? a.PS_Pessoas_Sac.des_pessoa : a.des_nome != null ? a.des_nome : "Não atribuído").FirstOrDefault();
            string _origem = db.PS_Origem_Sac.Where(a => a.cod_origem == sac.cod_origem).Select(a => a.des_nome).FirstOrDefault();
            string _tipo = db.PS_Tipo_Sac.Where(a => a.cod_tipo == sac.cod_tipo).Select(a => a.des_nome).FirstOrDefault();


            var data_pass = new Ps_Sac_Procedimento_Model
            {
                ProcedimentoAdm = new ProcedimentoAdm(),
                cod_sac = sac.cod_sac,
                Nome_Sac = _nome,
                des_assunto = sac.des_assunto,
                origem = _origem,
                tipo = _tipo,
                solicitacao = sac.des_assunto,
                _total_aberto = sac._total_aberto


            };

            //sac.PS_Pessoas_Sac != null ? sac.PS_Pessoas_Sac.des_pessoa : sac.des_nome != null ? sac.des_nome : "Não atribuído"
            string _cod_pessoa = sac.cod_pessoa.ToString();

            if (sac.tp_pessoa == "C")
            {
                _cod_pessoa = db.Contatos.Where(p => p.cod_contato == Convert.ToInt32(_cod_pessoa)).Select(a => a.DadosDoCrm.cod_pessoa).ToString();
            }
            else if (sac.tp_pessoa == "E")
            {
                _cod_pessoa = sac.cod_pessoa.ToString();
            }
            else if (sac.tp_pessoa == "L")
            {
                _cod_pessoa = null;
            }
            int teste = Convert.ToInt32(_cod_pessoa);

            ViewBag.cd_cadastro = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == teste).ToList(), "CD_CADASTRO", "RAZAO", _cod_pessoa);
            ViewBag.cd_tipo = new SelectList(db.TP_PROCEDIMENTO.Where(a => a.ATIVO == "S").ToList(), "CD_TIPO", "DES_TIPO", data.ProcedimentoAdm.CD_TIPO);
            ViewBag.cd_departamento = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S").ToList(), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", data.ProcedimentoAdm.CD_DEPARTAMENTO);
            ViewBag.cod_transportador = new SelectList(db.TRANSPORTADOR.ToList(), "COD_TRANSPORTADOR", "RAZAO", data.ProcedimentoAdm.CD_TRANSPORTADOR);
            ViewBag.tp_pessoa = new SelectList(db.Combo.Where(p => p.TIPO == 6), "Value", "Text");
            ViewBag.cod_tipo = new SelectList(db.PS_Tipo_Sac.ToList(), "cod_tipo", "des_nome");
            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac.ToList(), "cod_situacao", "des_nome");
            ViewBag.cod_origem = new SelectList(db.PS_Origem_Sac.ToList(), "cod_origem", "des_nome");
            ViewBag.cod_classe = new SelectList(db.PS_Classe_Sac.ToList(), "cod_classe", "des_nome");
            ViewBag.motivoid = new SelectList(db.Tp_Procedimento_Motivos.Where(a => a.COD_TIPO == 0).ToList(), "cod_tipo", "des_nome");

            #endregion
            #region logic_to_add_procedimetno


            int cd_tipo_procedimento = data.ProcedimentoAdm.CD_TIPO;
            int? nf_fox = data.ProcedimentoAdm.NF_FOX.GetValueOrDefault(0);
            int? nf_cliente = data.ProcedimentoAdm.NF_CLIENTE.GetValueOrDefault(0);

            string NFFOXOBRIGATORIA = (from a in db.TP_PROCEDIMENTO.Where(a => a.CD_TIPO == cd_tipo_procedimento) select a.SOL_NF_OBRIGATORIA).FirstOrDefault().ToString();
            string NFCLIOBRIGATORIA = (from a in db.TP_PROCEDIMENTO.Where(a => a.CD_TIPO == cd_tipo_procedimento) select a.SOL_NF_CLIENTE_OBRIGATORIA).FirstOrDefault().ToString();

            if (NFFOXOBRIGATORIA == "S" && nf_fox == 0)
            {
                ModelState.AddModelError("ProcedimentoAdm_NF_FOX", "*");
                return View(data);
            }

            if (NFCLIOBRIGATORIA == "S" && nf_cliente == 0)
            {
                ModelState.AddModelError("ProcedimentoAdm_NF_CLIENTE", "*");
                return View(data);
            }




            if (ModelState.IsValid)
            {
                data.ProcedimentoAdm.NF_FOX = nf_fox;
                string dta_nf = "";
                string cod_oper = "";
                int cod_transportador = 0;

                if (data.ProcedimentoAdm.NF_FOX != 0)
                {
                    dta_nf = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.DT_EMISSAO).FirstOrDefault().ToString();
                    cod_oper = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.COD_OPER).FirstOrDefault().ToString();
                    cod_transportador = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.CD_TRANSPORTADOR).FirstOrDefault();

                }
                else
                {
                    dta_nf = "";
                    cod_oper = "";
                    cod_transportador = 0;
                }

                Int32? intCD_PROCEDIMENTO = db.ProcedimentoAdm.Max(s => (Int32?)s.CD_PROCEDIMENTO);

                if (intCD_PROCEDIMENTO != null)
                {
                    intCD_PROCEDIMENTO++;
                }
                else
                {
                    intCD_PROCEDIMENTO = 1;
                }

                data.ProcedimentoAdm.CD_PROCEDIMENTO = (Int32)intCD_PROCEDIMENTO;
                data.ProcedimentoAdm.DTA_ABERTURA = DateTime.Now;
                data.ProcedimentoAdm.ID_SITUACAO = 1;
                data.ProcedimentoAdm.DTA_NF_FOX = dta_nf;
                data.ProcedimentoAdm.COD_OPER = cod_oper;
                data.ProcedimentoAdm.CD_DEPARTAMENTO = data.ProcedimentoAdm.CD_DEPARTAMENTO;
                data.ProcedimentoAdm.CD_TRANSPORTADOR = cod_transportador;

                if (files != null)
                {
                    foreach (var a in files)
                    {
                        if (a != null)
                        {
                            if (a.ContentLength > 0)
                            {

                                int tamanho = (int)a.InputStream.Length;
                                string contentype = a.ContentType;
                                byte[] arq = new byte[tamanho];

                                a.InputStream.Read(arq, 0, tamanho);
                                byte[] arqUp = arq;

                                

                                int intCD_PROCEDIMENTOARQ = db.Database.SqlQuery<Int32>("select PROCEDIMENTOARQSEQ.NextVal from dual ").FirstOrDefault<Int32>();


                                string NomeArquivo = data.ProcedimentoAdm.CD_PROCEDIMENTO.ToString() + '_' + intCD_PROCEDIMENTOARQ.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));
                                var path = Path.Combine(Server.MapPath(Settings.caminho_arquivos_procedimento), NomeArquivo);

                                ProcedimentoAdmArq doc = new ProcedimentoAdmArq
                                {
                                    ID_ARQ = intCD_PROCEDIMENTOARQ,
                                    CAMINHO = NomeArquivo,
                                    CD_PROCEDIMENTO = data.ProcedimentoAdm.CD_PROCEDIMENTO,
                                    DES_CONTENTTYPE = contentype,
                                    DES_IMAGEM = arqUp,
                                    NOME_ARQUIVO = a.FileName
                                };
                                db.ProcedimentoAdmArq.Add(doc);

                                //Encoding enc8 = Encoding.UTF8;
                                //string sqlFile = string.Format(" INSERT INTO PROCEDIMENTOARQ VALUES ({0},{1},\'{2}\',\'{3}\',\'{4}\',\'{5}\')",
                                //    intCD_PROCEDIMENTOARQ,
                                //    data.ProcedimentoAdm.CD_PROCEDIMENTO,
                                //    NomeArquivo,
                                //    BitConverter.ToString(arqUp),
                                //    contentype,
                                //    a.FileName);
                                //db.Database.ExecuteSqlCommand(sqlFile);

                                a.SaveAs(path);



                            }
                        }
                    }
                }

                //}


                int cd_regional = (from a in db.Clientes.Where(a => a.CD_CADASTRO == data.ProcedimentoAdm.CD_CADASTRO) select a.CD_REGIONAL).FirstOrDefault();
                data.ProcedimentoAdm.CD_USUARIO = cd_usuario;
                data.ProcedimentoAdm.CD_USUARIO_ALTERACAO = cd_usuario;
                data.ProcedimentoAdm.OBSATENDIMENTO = "";
                data.ProcedimentoAdm.CD_REGIONAL = cd_regional;
                data.ProcedimentoAdm.CD_ANEXO = 1;


                #endregion

                //                db.ProcedimentoAdm.Add(procedimentoadm);
                try
                {
                    db.ProcedimentoAdm.Add(data.ProcedimentoAdm);
                    PS_Sac sac_to_update = db.PS_Sac.Find(sac.cod_sac);
                    sac_to_update.cod_estagio = sac.cod_estagio;
                    sac_to_update.cod_situacao = sac.cod_situacao;
                    sac_to_update.des_assunto = sac.des_solucao;
                    sac_to_update.dta_finalizacao = sac.dta_finalizacao;
                    sac_troca_estagio finaliza_ultima_troca = AddDataSaidaEstagioAnterior(sac.cod_sac);
                    db.Entry(sac_to_update).State = EntityState.Modified;
                    db.Entry(finaliza_ultima_troca).State = EntityState.Modified;
                    SacProcedimento sacprocedimento = new SacProcedimento { cod_sac = sac_to_update.cod_sac, cod_procedimento = data.ProcedimentoAdm.CD_PROCEDIMENTO };
                    db.SacProcedimento.Add(sacprocedimento);


                    db.SaveChanges();
                    SendEmail email = new SendEmail();
                    email.EnviarEmail(data.ProcedimentoAdm.CD_PROCEDIMENTO, "Create");


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
                    return View(data);
                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        ModelState.AddModelError("", error.Exception.Message);
                    }
                }
                return View(data);

            }


            return RedirectToAction("Details", new { id = data.cod_sac });
        }



        public JsonResult ReadNF(int nr_nota, int cd_cadastro)
        {
            // var list = (List<int>)Session["oRegional"];
            var Notas = db.eNota.Where(a => a.NR_NOTA == nr_nota && a.CD_CADASTRO == cd_cadastro).FirstOrDefault();
            //IEnumerable<eNota> notas = db.eNota;

            //if (cd_cadastro != 0 )
            //{ notas.Where(a => a.CD_CADASTRO ==  cd_cadastro) };

            //     notas.Where(a => a.NR_NOTA == nr_nota && a.CD_CADASTRO == cd_cadastro);

            //  var Notas = notas.FirstOrDefault();

            string _msg = "";
            int _procedimento = 0;

            if (db.ProcedimentoAdm.Any(a => a.NF_FOX == nr_nota))
            {
                _procedimento = db.ProcedimentoAdm.Where(a => a.NF_FOX == nr_nota).Select(a => a.CD_PROCEDIMENTO).FirstOrDefault();
                _msg =  $" A Nota Fiscal {nr_nota} já está vinculada ao procedimento <a target='blank' href='http://procedimento.grupofoxlux.com.br/Procedimento/ProcedimentoAdm/Details/{_procedimento}'>{_procedimento}</a>, por favor verifique ";
            }
            if (Notas == null) { Notas = new eNota(); }


            var objRetorno = new
            {
                nota = Notas,
                msg = _msg,
                procedimento = _procedimento
            };

            
            return Json(objRetorno, JsonRequestBehavior.AllowGet);

            

        }



    }
}
