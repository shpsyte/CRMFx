using CRM.App_Helpers;
using CRM.Extends;
using Domain.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class CampanhaAnaliseController : BasePublicController
    {
        protected SendEmail _email = new SendEmail();
        string sql_base = " SELECT nvl(SUM(Cedia.Vlr_Total),0) " +
                          "  FROM Ce_Diarios CeDia INNER JOIN NS_NOTAS Nota ON Nota.Num_Seq = CeDia.Num_Seq_Ns " +
                          " WHERE Nota.Dta_Emissao BETWEEN \'{0}\'  AND \'{1}\' " +
                          "   AND Nota.cod_cliente = {2} ";

        string SqlBase = " select * from CampanhaRegional  ";


        //
        // GET: /AnaliseCampanha/

        public ActionResult List()
        {
            ViewBag.tipoacao = new SelectList(db.TipoAcao, "tipoacaoid", "des_acao", string.Empty);
            ViewBag.segmentos = new SelectList(db.Segmentos, "segmentoid", "des_segmento", string.Empty);
            ViewBag.estagio = new SelectList(db.Estagio.Where(a => a.ind_liberado == "N"), "estagioId", "descricao");
            ViewBag.formapgto = new SelectList(db.FormaPgto, "formapgtoid", "des_forma");

            return View();
        }

        public ActionResult ReadWorkFlow(ParametersDataTable param)
        {
            int total_de_linhas = db.Database.SqlQuery<int>(string.Format(" select Count(*) from campanhamarketing where statusid = {0} ", 3)).FirstOrDefault();
            IEnumerable<CampanhaMarketing> filteredCompanies;
            filteredCompanies = db.CampanhaMarketing.Where(a => a.statusId == 3);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            //Func<CampanhaMarketing, string> orderingFunction = (c =>
            //                                               sortColumnIndex == 0 ? c.campanhaID.ToString() :
            //                                               sortColumnIndex == 1 ? c.campanhaID.ToString() :
            //                                               sortColumnIndex == 2 ? c.des_nome :
            //                                               sortColumnIndex == 3 ? c.Ps_Pessoas.des_pessoa :
            //                                               sortColumnIndex == 4 ? c.Segmentos.des_segmento.ToString() :
            //                                               sortColumnIndex == 5 ? c.TipoAcao.des_acao.ToString() :
            //                                                c.campanhaID.ToString()
            //                                             );


            string codcliente = Convert.ToString(Request["sSearch_5"]);
            string cliente = Convert.ToString(Request["sSearch_6"]);


            string segmentos = Convert.ToString(Request["sSearch_3"]);
            string tipoacao = Convert.ToString(Request["sSearch_4"]);
            string codcampanha = Convert.ToString(Request["sSearch_0"]);




            if (!string.IsNullOrEmpty(segmentos))
                filteredCompanies = filteredCompanies.Where(c => c.Segmentos.des_segmento.ToUpper().Contains(segmentos.ToUpper()));

            if (!string.IsNullOrEmpty(tipoacao))
                filteredCompanies = filteredCompanies.Where(c => c.TipoAcao.des_acao.ToUpper().Contains(tipoacao.ToUpper()));

            if (!string.IsNullOrEmpty(cliente))
                filteredCompanies = filteredCompanies.Where(c => c.Ps_Pessoas != null && c.Ps_Pessoas.des_pessoa.ToUpper().Contains(cliente.ToUpper())) ;

            if (!string.IsNullOrEmpty(codcliente))
                filteredCompanies = filteredCompanies.Where(c => c.DadosDoCrm != null && c.DadosDoCrm.cod_pessoa.ToUpper().Contains(codcliente.ToUpper()));
            if (!string.IsNullOrEmpty(codcampanha))
                filteredCompanies = filteredCompanies.Where(c => c.campanhaID == Convert.ToInt32(codcampanha));


            //var sortDirection = Request["sSortDir_0"]; // asc or desc
            //if (sortDirection == "asc")
            //    filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            //else
            //    filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new
                         {
                             campanhaid = c.campanhaID,
                             des_nome = c.des_nome,
                             cod_pessoa = c.cod_pessoa,
                             nome_pessoa = c.Ps_Pessoas == null ? "" : c.Ps_Pessoas.des_pessoa,
                             cod_representeante = c.cod_representante,
                             nome_representante = c.Ps_Representantes == null ? "" : c.Ps_Representantes.des_pessoa,
                             des_email = c.des_email,
                             des_segmetno = c.Segmentos == null ? "" : c.Segmentos.des_segmento,
                             des_acao = c.TipoAcao == null ? "" : c.TipoAcao.des_acao,
                             dta_inicial = c.dta_inicial.HasValue ? c.dta_inicial.Value.ToShortDateString() : "",
                             dta_final = c.dta_final.HasValue ? c.dta_final.Value.ToShortDateString() : "",
                             ind_renova = c.ind_renova,
                             vlr_meta = c.vlr_meta.HasValue ? c.vlr_meta.Value.ToString("c") : "0",
                             vlr_contrato = c.vlr_contrato.HasValue ? c.vlr_contrato.Value.ToString("c") : "0",
                             vlr_custo_medio = c.vlr_custo_medio.HasValue ? c.vlr_custo_medio.Value.ToString("c") : "0",
                             tip_apuracao = c.tip_apuracao,
                             des_fomra_pgto = c.FormaPgto == null ? "" : c.FormaPgto.des_forma,
                             des_agencia = c.des_agencia,
                             des_banco = c.des_banco,
                             des_conta = c.des_conta,
                             des_obervacao = c.des_observacao,
                             estagio = c.Estagio.descricao,
                             status = c.Status.descricao,
                             acao = "",
                             minimum_details = "",
                             cod_regional = c.cod_regional
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

        public PartialViewResult GetMinimumDetails(int id)
        {

            string id_to_string = id.ToString();
            var Campanha = db.CampanhaMarketing.Find(id);


            DateTime? dta_inicio = Campanha.dta_inicial;
            DateTime? dta_fim = Campanha.dta_final;
            int cod_cliente = Campanha.cod_pessoa;

            if (Campanha.segmentoid == 1)
            { dta_fim = System.DateTime.Now.AddYears(7); }

            if (!dta_fim.HasValue)
            { dta_fim = System.DateTime.Now.AddYears(7); }

            decimal vlr_faturado = db.Database.SqlQuery<decimal>(string.Format(sql_base, dta_inicio.Value.ToShortDateString(), dta_fim.Value.ToShortDateString(), cod_cliente)).FirstOrDefault();

            decimal per_atingido = 0;

            if (Campanha.segmentoid == 2)
            { per_atingido = Math.Round(vlr_faturado / Campanha.vlr_meta.Value, 4); }

            CampanhaMarketingReviewModel data = new CampanhaMarketingReviewModel
            {
                CampanhaMarketing = Campanha,
                //ListaComentarios = db.ListaComentarios.Where(a => a.cod_interno == id_to_string && a.tipo_nota == "CAMPANHA").ToList(),
                // ListaEstagio = db.CampanhaMarketingEstagios.Where(a => a.campanhaId == id).OrderBy(p => p.num_seq).ToList(),
                DadosDoCrm = db.Dados_crm.Where(a => a.cod_pessoa == Campanha.cod_pessoa_string).FirstOrDefault(),
                vlr_faturado = vlr_faturado,
                per_atingido = per_atingido,
                vlr_total_pago = db.CampanhaMarketingPgto.Where(a => a.campanhaid == id).Select(a => a.vlr_pgto).DefaultIfEmpty(0).Sum()
            };


            return PartialView(data);
        }

        public ActionResult Regional()
        {
            ViewBag.ano = new SelectList((from e in db.CampanhaMarketing
                                       group e by e.dta_inclusao.Year into g
                                       select new { Year = g.Key, Events = g }), "Year", "Year", "2017");
            ViewBag.statusId = new SelectList(db.Status, "statusId", "descricao", "3");

            return View();
        }

        public ActionResult RegionalFilter(int ano, int? statusId)
        {
            var sstatusId = statusId ?? 0;
            db.Database.ExecuteSqlCommand(string.Format("Begin SpcAnaliseRegional({0},{1}); end;", ano, sstatusId));
           // int sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();
            var dados = db.Database.SqlQuery<CampanhaRegionalViewModel>(SqlBase/*string.Format(SqlBase,sessao)*/).ToList();
            return View(dados);
        }


        [HttpPost]
        public ActionResult SolicitaPgtoCampanha(int id, int estagio, 
            int formapgto, string Obs, decimal vlr_pgto, string ind_total, string des_agencia = "", string des_banco = "", string des_conta = "")
        {
            CampanhaMarketing data = db.CampanhaMarketing.Find(id);
            string nome_forma = db.FormaPgto.Where(a => a.formapgtoid == formapgto).Select(a => a.des_forma).FirstOrDefault();

            if (formapgto == 4)
            {
                if ((string.IsNullOrEmpty(des_agencia)) || (string.IsNullOrEmpty(des_banco)) || (string.IsNullOrEmpty(des_conta)))
                {
                    return Json(new { id = data.campanhaID, Msg = "Dados da conta devem ser informados", hasError = true });
                }

            }

            try
            {
                string emailcopia = db.Usuario.Where(a => a.CD_USUARIO == cd_usuario).Select(a => a.EMAIL).FirstOrDefault();
                string nomecopia  = db.Usuario.Where(a => a.CD_USUARIO == cd_usuario).Select(a => a.NOME).FirstOrDefault();


                _email.EnviarEmailSolicitacaoPgto
                    (data.campanhaID, "SolicitacaoPgto.htm", estagio, Obs, nome_forma, vlr_pgto.ToString("c"),
                    ind_total.ToString(), emailcopia, nomecopia, des_agencia,  des_banco,  des_conta);


                return Json(new { id = data.campanhaID, Msg = "Ok, enviado e-mail com sucesso ", hasError = false,
                    Obs = Obs, nome_forma = nome_forma, vlr_pgto = vlr_pgto.ToString("c"),  int_total = ind_total.ToString()

                });
            }
            catch (Exception e)
            {
                return Json(new { id = data.campanhaID, Msg = e.Message, hasError = true });
            }

        }
   
        public ActionResult InformaPgtoCamapanha(int id)
        {
            ViewBag.formapgto = new SelectList(db.FormaPgto, "formapgtoid", "des_forma");

            CampanhaMarketingPgtoViewModel data = new CampanhaMarketingPgtoViewModel
            {
                CampanhaMarketing = db.CampanhaMarketing.Find(id),
                ListaPgto = db.CampanhaMarketingPgto.Where(a => a.campanhaid == id).OrderBy(p => p.campanhamarketingpgtoid).ToList()
                
            };
            return View(data);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InformaPgtoCamapanha(CampanhaMarketingPgtoViewModel data)
        {
            ViewBag.formapgto = new SelectList(db.FormaPgto, "formapgtoid", "des_forma", data.CampanhaMarketingPgto.formapgtoid);

            if (data.CampanhaMarketingPgto.ind_total == "S")
            {
                var dataupdate = db.CampanhaMarketing.Find(data.CampanhaMarketing.campanhaID);
                dataupdate.statusId = 4;
                dataupdate.estagioId = 61;
                db.Entry(dataupdate).State = EntityState.Modified;
            }


            ModelState.Clear();
            data.CampanhaMarketingPgto.dta_inclusao = System.DateTime.Now;
            data.CampanhaMarketingPgto.campanhamarketingpgtoid = db.Database.SqlQuery<Int32>("select CampanhaMarketingPgtoSeq.NextVal from dual ").FirstOrDefault<Int32>();
            data.CampanhaMarketingPgto.campanhaid = data.CampanhaMarketing.campanhaID;
            data.CampanhaMarketingPgto.cod_usuario = cd_usuario;

            data.CampanhaMarketingPgto.des_agencia = ReturnSpaceIfNull(data.CampanhaMarketingPgto.des_agencia);
            data.CampanhaMarketingPgto.des_banco = ReturnSpaceIfNull(data.CampanhaMarketingPgto.des_banco);
            data.CampanhaMarketingPgto.des_conta = ReturnSpaceIfNull(data.CampanhaMarketingPgto.des_conta);

            if (Request.Files.Count > 0)
            {

                int tamanho = (int)Request.Files[0].InputStream.Length;
                string contentype = Request.Files[0].ContentType;
                byte[] arq = new byte[tamanho];

                Request.Files[0].InputStream.Read(arq, 0, tamanho);
                byte[] arqUp = arq;
                data.CampanhaMarketingPgto.des_imagem = arqUp;
                data.CampanhaMarketingPgto.des_contentype = contentype;

                
            }
            


            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.CampanhaMarketingPgto.Add(data.CampanhaMarketingPgto);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("PagamentoConfirmado",new { id = data.CampanhaMarketing.campanhaID});
                }
                catch (DbEntityValidationException e)
                {
                    data.ListaPgto = db.CampanhaMarketingPgto.Where(a => a.campanhaid == data.CampanhaMarketing.campanhaID).OrderBy(p => p.campanhamarketingpgtoid).ToList();
                    return View(data);
                }
                
            }

            data.ListaPgto = db.CampanhaMarketingPgto.Where(a => a.campanhaid == data.CampanhaMarketing.campanhaID).OrderBy(p => p.campanhamarketingpgtoid).ToList();
            return View(data);

            
        }

        public ActionResult Download(int id)
        {
            var doc = db.CampanhaMarketingPgto.Where(a => a.campanhamarketingpgtoid == id).Select(a => a.des_imagem).FirstOrDefault();
            var contenttype = db.CampanhaMarketingPgto.Where(a => a.campanhamarketingpgtoid == id).Select(a => a.des_contentype).FirstOrDefault();

            var docs = (from a in db.CampanhaMarketingPgto
                       where a.campanhamarketingpgtoid == id
                       select new 
                       {
                           doc = a.des_imagem,
                           tipo = a.des_contentype
                       }).ToList();

            byte[] imagedata = (byte[])docs[0].doc;

            //return File(imagedata, docs[0].tipo);


            if (imagedata == null)
            {
                return File("~/Arquivos/blank.gif", "image/png");
            }
            else
                return File(imagedata, docs[0].tipo);


        }

        public ActionResult PagamentoConfirmado(int id)
        {
            return View(id);
        }


        private string ReturnSpaceIfNull(string p)
        {
            if (string.IsNullOrEmpty(p))
            {
                return ".";
            }
            else
                return p;
        }



        public ActionResult PgtoCamapanha(int id)
        {
            var data = db.CampanhaMarketingPgto.Where(a => a.campanhaid == id).ToList();
            return View(data);

        }


    }
}
