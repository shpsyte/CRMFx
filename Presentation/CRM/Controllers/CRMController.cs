using CRM.App_Helpers;
using CRM.Extends;
using CRM.Models;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CRM.Controllers
{
    [CustomAuthorize(AccessLevel = "Admin;User")]
    public class CRMController : BasePublicController
    {


        //public JsonResult ReadNewSac()
        //{

        //    var Notas = db. .eNota.Where(a => a.NR_NOTA == nr_nota && a.CD_CADASTRO == cd_cadastro).FirstOrDefault();
        //    //IEnumerable<eNota> notas = db.eNota;

        //    //if (cd_cadastro != 0 )
        //    //{ notas.Where(a => a.CD_CADASTRO ==  cd_cadastro) };

        //    //     notas.Where(a => a.NR_NOTA == nr_nota && a.CD_CADASTRO == cd_cadastro);

        //    //  var Notas = notas.FirstOrDefault();

        //    if (Notas == null) { Notas = new eNota(); }
        //    return Json(Notas, JsonRequestBehavior.AllowGet);



        //}


        public ActionResult Index()
        {
            if (!Settings.EnableDashborad)
            {
                return RedirectToAction("IndexSD");
            }
            ViewBag.sac_por_tipo = db.w_sac_por_tipo.ToList();

          
            return View();
        }

        public ActionResult IndexSD()
        {

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


        public ActionResult GetSac(ParametersDataTable param, string param_search = "")
        {
            string s = param_search.ToUpper();

            int total_de_linhas = db.Database.SqlQuery<int>(" select Count(*) from ps_sac  ").FirstOrDefault();

           
            IEnumerable<vw_Sac> filteredCompanies;

            if ((!string.IsNullOrEmpty(param_search)))
            {
                filteredCompanies = db.vw_Sac.AsNoTracking().Where
                                           (a => a.cod_pessoa.Contains(s)
                                         || a.cod_sac.Contains(s)
                                         || a.des_pessoa.Contains(s)
                                         || a.des_assunto.Contains(s)
                                         );
            }
            else
            {
                filteredCompanies = db.vw_Sac.AsNoTracking();
            }

            

            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new
                         {
                             cod_pessoa = c.cod_pessoa,
                             des_pessoa = c.des_pessoa,
                             cod_sac = c.cod_sac,
                             des_assunto = c.des_assunto,
                             des_estagio = c.des_estagio
                         };


            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total_de_linhas,
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }


        public ActionResult GetGarantia(ParametersDataTable param, string param_search = "")
        {
            string s = param_search.ToUpper();

            int total_de_linhas = db.Database.SqlQuery<int>(" select Count(*) from ps_sac  ").FirstOrDefault();

            IEnumerable<w_garantia> filteredCompanies ;

            if ((!string.IsNullOrEmpty(param_search)))
            {
                filteredCompanies = db.w_garantia.AsNoTracking().Where
                                    (a => a.garantiaid.Contains(s)
                                  || a.des_pessoa.Contains(s)
                                  || a.num_nf_cliente.Contains(s)
                                  || a.cod_cliente.Contains(s)
                                  || a.cod_representante.Contains(s)
                                  );
            }
            else
            {
                filteredCompanies = db.w_garantia.AsNoTracking();
            }



            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new
                         {
                             cod_cliente = c.cod_cliente,
                             des_pessoa = c.des_pessoa,
                             cod_representante = c.cod_representante,
                             garantiaid = c.garantiaid,
                             num_nf_cliente = c.num_nf_cliente,
                             obs = c.obs

                         };


            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total_de_linhas,
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }



        public ActionResult Search(string param_search)
        {
            /*SELECT score(1), garantiaid, cod_cliente
                from garantia
                WHERE 1 = 1 and(CONTAINS(obs, 'garantia', 1) > 0 or CONTAINS(num_nf_cliente, '7019') > 0);
                            select obs from garantia
                            where obs is not null
            --create index ps_sacidxassuntop on ps_sac(des_assunto) indextype is ctxsys.context;
            */

            string s = param_search.ToUpper();

            var leads = db.vw_Ps_Leads.AsNoTracking().Where(
                        //a => a.cod_lead.ToString().Contains(s)
                        a => a.des_nome.ToUpper().Contains(s)
                        || (a.des_email != null && a.des_email.ToUpper().Contains(s))
                        ).ToList();

            var contatos = db.vw_Contatos.AsNoTracking().Where(a =>
                         //a => a.cod_contato.Contains(s)
                         a.des_nome.ToUpper().Contains(s)
                        || a.cod_empresa.ToUpper().Contains(s)
                        || a.des_empresa.ToUpper().Contains(s)
                        || a.des_email.ToUpper().Contains(s)
                        || a.cnpj.ToUpper().Contains(s)
                        ).ToList();


            //var sac = db.vw_Sac.AsNoTracking().Where(
            //       a => a.des_pessoa.Contains(s)
            //       || a.cod_pessoa.Contains(s)
            //       || a.cod_sac.Contains(s)
            //       || a.des_assunto.Contains(s)
            //    ).ToList();



            //var garantia = db.w_garantia.AsNoTracking().Where(
            //       a => a.garantiaid.Contains(s)
            //       || a.des_pessoa.Contains(s)
            //       || a.num_nf_cliente.Contains(s)
            //       || a.cod_cliente.Contains(s)
            //       || a.cod_representante.Contains(s)
            //    ).ToList();


            var pessoas = db.vw_pessoas.AsNoTracking().Where(
                   a => a.cod_pessoa.Contains(s)
                   || a.des_pessoa.Contains(s)
                   || a.des_email.Contains(s)
                ).ToList();


            SearchModel search = new SearchModel
            {
                param = param_search,
                Leads = leads,
                Contatos = contatos,
                Sac = null,
                Garantia = null,
                Pessoas = pessoas
            };



            //Leads
            //string sql_leasd = " select to_char(a.cod_lead), a.des_nome, a.des_email, a.des_fone, a.des_documento from ps_leads a where 1=1 ";
            //if (!string.IsNullOrEmpty(param))
            //{
            //    sql_leasd += string.Format(" and ( (a.cod_lead) LIKE ('%{0}%')", param);
            //    sql_leasd += string.Format(" or upper(a.des_nome) LIKE ('%{0}%')", param);
            //    sql_leasd += string.Format(" or upper(a.des_email) LIKE ('%{0}%') )", param);
            //}
            //var allleads = db.Database.SqlQuery<V_Ps_Leads>(sql_leasd);

            ////contatos
            //sql_leasd = "";
            //sql_leasd = "   select to_char(a.cod_contato), a.des_nome, a.cod_conta cod_empresa,  " +
            //            " (Select des_pessoa from dados_crm where cod_pessoa = a.cod_conta and rownum = 1) des_empresa, " +
            //            " (Select to_char(cgc_cpf) from dados_crm where cod_pessoa = a.cod_conta and rownum = 1 ) cnpj, des_email "+
            //            " from ps_contatos_crm a where 1=1 "; 

            //if (!string.IsNullOrEmpty(param))
            //{
            //    sql_leasd += string.Format(" and ( (a.cod_contato) LIKE ('%{0}%')", param);
            //    sql_leasd += string.Format(" or upper(a.des_nome) LIKE ('%{0}%')", param);
            //    sql_leasd += string.Format(" or upper(a.des_email) LIKE ('%{0}%') )", param);
            //}
            //var allcontatos = db.Database.SqlQuery<V_Contatos>(sql_leasd);




            return View(search);
        }


      



        public JsonResult GetData(int id)
        {
            List<Object> Adicionado = new List<Object>();
            if (id == 1)
            {
                var Data = db.Grafico1.Where(a => a.QTDEATIVAS > 0).OrderBy(a => a.QTDEATIVAS);
                int linha = 1;
                foreach (var item in Data.OrderBy(a => a.QTDEATIVAS))
                {
                    Adicionado.Add(new { Ordem = linha, Codigo = item.CD_DEPARTAMENTO, Nome = item.DESC_DEPARTAMENTO, Qtde = item.QTDEATIVAS });
                    linha++;
                }
            }
            if (id == 2)
            {
                var Data = db.W_TopProcedimentoByDepto;
                int linha = 1;
                foreach (var item in Data.OrderBy(a => a.maiorproc))
                {
                    Adicionado.Add(new { Ordem = linha, Codigo = item.cd_departamento, Nome = item.departamento, Qtde = item.maiorproc });
                    linha++;
                }
            }

            if (id == 3)
            {
                var Data = db.W_QTDE_SAC_DIA;
                int linha = 1;
                int qtde;
                for (var i = 1; i <= 31; i++)
                {

                    qtde = Data.Where(p => p.dia == i).Select(p => p.qtde).FirstOrDefault();
                    Adicionado.Add(new { Ordem = i, Codigo = i, Qtde = qtde });
                }

                //foreach (var item in Data.OrderBy(a => a.dia))
                //{
                //   Adicionado.Add(new { Ordem = item.dia, Codigo = item.dia, Qtde = item.qtde });
                //   linha++;
                /// }



            }



            return Json(Adicionado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GameError()
        { return PartialView(); }
        //public ActionResult Error()
        //{
        //    HandleErrorInfo data;
        //    return View();
        //}
    }
}
