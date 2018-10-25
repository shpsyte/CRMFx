using CRM.App_Helpers;
using CRM.Extends;
using Danzor.Print;
using Domain.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.Web.Mvc.Html;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;
using static Domain.Entity.XMLElement;

namespace CRM.Controllers
{
    [Services.Functions.NoCache]
    public class GarantiasController : BasePublicController
    {

        string SQL_GET_GARANTIA_TOTAL_POR_REPRES = " select Count(*) Qtde,a.cod_representante, b.des_pessoa, sum(a.vlr_garantia) vlr_garantias, min(a.dta_previsao_coleta) dta_menor_coleta   ,  " +
                                           " fnGetGarantias(a.cod_representante) lista_nf_garantias  " +
                                           " from Garantia a " +
                                           " Left Outer Join Representantes b on a.cod_representante = b.COD_PESSOA " +
                                              " Where a.ind_emitido_nf = 'S' " +
                                              "   and a.Ind_Emitido_Coleta = 'S' " +
                                              "   and a.ind_cancelada = 'N' " +
                                                "   and a.dta_finalizacao is null  " +
                                          " Group by a.cod_representante , b.DES_PESSOA ";


        string SQL_GET_GARANTIA_POR_REPRES = "  select a.cod_representante, a.garantiaid garantiaid , d.des_nome, a.cod_cliente, c.razao, a.dta_inclusao,  " +
                                            " a.num_nf_cliente, a.obs, nvl(a.vlr_garantia,0) vlr_garantia, a.num_coleta, a.dta_previsao_coleta, a.dta_coleta, a.obscoleta, a.volumes, a.especies, a.cod_procedimento_vinculado  " +
                                            "   from Garantia a " +
                                            "   Left Outer Join Representantes b on a.cod_representante = b.COD_PESSOA " +
                                            "   Left Outer Join Clientes c on c.CD_CADASTRO = a.cod_cliente " +
                                            "    Left Outer Join Ge_Status_Garantia d on d.cod_status = a.cod_status " +
                                            "   Where a.ind_emitido_nf = 'S' " +
                                                "   and a.Ind_Emitido_Coleta = 'S' " +
                                                "   and a.dta_finalizacao is null  " +
                                            "     and a.ind_cancelada = 'N' " +
                                            " and a.cod_representante = {0} " +
                                                " ORDER BY A.garantiaid ";


        public ActionResult List(/*bool ind_aberta = true, bool ind_finalizadas = false*/)
        {
            //var allItem = db.Garantia.ToList();
            //List<int> StatusAtivo = db.GE_Status_Garantia.Where(p => p.ind_finalizacao == "N").Select(a => a.Cod_Status).ToList();
            //List<int> StatusFinalizada = db.GE_Status_Garantia.Where(p => p.ind_finalizacao == "S").Select(a => a.Cod_Status).ToList();

            IEnumerable<Garantia> filterGarantia = db.Garantia;

            //if (ind_aberta)
            //{
            //    filterGarantia = db.Garantia.Where(c => StatusAtivo.Contains((int)c.cod_status));
            //} 
            //if (ind_finalizadas)
            //{
            //    filterGarantia = db.Garantia.Where(c => StatusFinalizada.Contains((int)c.cod_status));
            //}

            filterGarantia = db.Garantia.Where(c => c.ind_emitido_nf == "N" && c.ind_emitido_coleta == "N" && c.ind_cancelada == "N");
            var data = filterGarantia.ToList().OrderByDescending(p => p.garantiaid);
            return View(data);


        }
        public ActionResult ListWithInvoiceCustomer(/*bool ind_aberta = true, bool ind_finalizadas = false*/)
        {
            //var allItem = db.Garantia.ToList();
            //List<int> StatusAtivo = db.GE_Status_Garantia.Where(p => p.ind_finalizacao == "N").Select(a => a.Cod_Status).ToList();
            //List<int> StatusFinalizada = db.GE_Status_Garantia.Where(p => p.ind_finalizacao == "S").Select(a => a.Cod_Status).ToList();

            IEnumerable<Garantia> filterGarantia = db.Garantia;

            //if (ind_aberta)
            //{
            //    filterGarantia = db.Garantia.Where(c => StatusAtivo.Contains((int)c.cod_status));
            //} 
            //if (ind_finalizadas)
            //{
            //    filterGarantia = db.Garantia.Where(c => StatusFinalizada.Contains((int)c.cod_status));
            //}

            filterGarantia = db.Garantia.Where(c => c.ind_emitido_nf == "S" && c.ind_emitido_coleta == "N" && c.ind_cancelada == "N");
            ViewBag.cod_transportador = new SelectList(db.TRANSPORTADOR, "COD_TRANSPORTADOR", "RAZAO");
            var data = filterGarantia.ToList().OrderByDescending(p => p.garantiaid);
            return View(data);


        }


        public ActionResult RegistrarColetas(int? id)
        {
            IEnumerable<Garantia> filterGarantia = db.Garantia;
            filterGarantia = db.Garantia.Where(c => c.ind_emitido_nf == "S" && c.ind_emitido_coleta == "S" && c.ind_cancelada == "N" && c.dta_finalizacao == null);
            if (id.HasValue)
                filterGarantia = filterGarantia.Where(a => a.cod_representante == id.Value);

            var data = filterGarantia.ToList();

            return View(data);
        }

        public ActionResult ListWithOrderCollection(/*bool ind_aberta = true, bool ind_finalizadas = false*/)
        {

            var garantiasTemporario = db.Tmp_Garantia_Impressao.Where(a => a.cod_usuario == cd_usuario).ToList();

            garantiasTemporario.ForEach(tmp => db.Tmp_Garantia_Impressao.Remove(tmp));
            db.SaveChanges();


            var data = db.Database.SqlQuery<V_Garantia_Total_Representante>(SQL_GET_GARANTIA_TOTAL_POR_REPRES).ToList();
            return View(data);

        }


        //
        // GET: /TipoLead/Details/5
        //
        public JsonResult GetPessoas(int id)
        {

            var states = (from a in
                              db.Dados_crm.Where(p => p.cod_representante_cli == id)
                          select new SelectListItem
                          {
                              Value = a.cod_pessoa,
                              Text = string.Concat(a.des_fantasia, " - ", a.des_pessoa),
                              Selected = false
                          });

            if (id == 1 || id == 3)
            {
                states = (from a in db.Dados_crm
                          select new SelectListItem
                          {
                              Value = a.cod_pessoa,
                              Text = string.Concat(a.des_fantasia, " - ", a.des_pessoa),
                              Selected = false
                          }
                          );
            }



            return Json(new SelectList(states.OrderBy(A => A.Text), "Value", "Text"));
        }
        public ActionResult Create()
        {
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes.OrderBy(a => a.des_pessoa), "cod_pessoa", "des_pessoa");
            ViewBag.cod_cliente = new SelectList(db.Ps_Pessoas.Where(a => a.cod_pessoa == 0), "cod_pessoa", "des_pessoa");
            ViewBag.cod_status = new SelectList(db.GE_Status_Garantia, "cod_status", "des_nome", 1);
            return View();
        }
        // POST: /TipoLead/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Create(Garantia data, bool continueAdd, FormCollection form)
        {
            ViewBag.cod_cliente = new SelectList(db.Ps_Pessoas, "cod_pessoa", "des_pessoa", data.cod_cliente);
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes, "cod_pessoa", "des_pessoa", data.cod_representante);
            ViewBag.cod_status = new SelectList(db.GE_Status_Garantia, "cod_status", "des_nome", data.cod_status);

            if (data.cod_representante == 0)
            {
                ModelState.AddModelError("cod_representante", "Deve informar um representnate");
                return View(data);
            }

            if (data.cod_cliente == 0)
            {
                ModelState.AddModelError("cod_cliente", "Deve informar um cliente");
                return View(data);
            }


            ModelState.Clear();
            data.garantiaid = db.Database.SqlQuery<Int32>("select GarantiaSeq.NextVal from dual ").FirstOrDefault<Int32>();
            data.dta_inclusao = System.DateTime.Now;
            data.cod_usuario = cd_usuario;
            data.vlr_garantia = 0;
            data.vlr_ngarantia = 0;
            data.ind_cancelada = "N";
            data.ind_coleta_direta = "N";
            data.ind_emitido_coleta = "N";
            data.ind_emitido_nf = "N";
            data.num_coleta = "";
            data.dta_previsao_coleta = null;
            data.dta_coleta = null;
            data.protocolo = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + data.garantiaid.ToString();
            data.cod_transportador = null;


            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                Garantia_Troca_Status nova_linha = AddNovaLinhaTrocaEstagio(data.garantiaid, null, data.cod_status, data.obs);
                db.Garantia_Troca_Status.Add(nova_linha);

                db.Garantia.Add(data);
                db.SaveChanges();

                return continueAdd ? RedirectToAction("AddItem", "GarantiaItem", new { id = data.garantiaid, cod_cliente = data.cod_cliente }) : RedirectToAction("List");

            }
            return View(data);
        }

        private Garantia_Troca_Status AddNovaLinhaTrocaEstagio(int Garantiaid, int? cod_status_anterior, int? cod_status_novo, string obs)
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
        private Garantia_Troca_Status AddDataSaidaEstagioAnterior(int Garantiaid)
        {
            int? num_seq = db.Garantia_Troca_Status.Where(a => a.garantiaId == Garantiaid && a.dta_saida == null).Max(p => p.num_seq);
            Garantia_Troca_Status gat_troca_update = db.Garantia_Troca_Status.Find(num_seq, Garantiaid);
            gat_troca_update.dta_saida = System.DateTime.Now;
            return gat_troca_update;
        }


        public ActionResult Edit(int id)
        {
            Garantia data = db.Garantia.Find(id);
            bool _ind_finalizada = data.GE_Status_Garantia.ind_finalizacao == "S";

            if (_ind_finalizada)
            { return RedirectToAction("Details", new { id = id }); }


            ViewBag.cod_cliente = new SelectList(db.Ps_Pessoas, "cod_pessoa", "des_pessoa", data.cod_cliente);
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes, "cod_pessoa", "des_pessoa", data.cod_representante);
            ViewBag.cod_status = new SelectList(db.GE_Status_Garantia, "cod_status", "des_nome", data.cod_status);
            ViewBag.bloqueiaEstagio = "sim";

            if (data == null)
            {
                return InvokeHttpNotFound();
            }

            return View(data);
        }


        // POST: /TipoLead/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(Garantia data, bool continueAdd, bool isDelete)
        {

            ViewBag.cod_cliente = new SelectList(db.Ps_Pessoas, "cod_pessoa", "des_pessoa", data.cod_cliente);
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes, "cod_pessoa", "des_pessoa", data.cod_representante);
            ViewBag.cod_status = new SelectList(db.GE_Status_Garantia, "cod_status", "des_nome", data.cod_status);
            ViewBag.bloqueiaEstagio = "sim";



            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return continueAdd ? RedirectToAction("Edit", new { id = data.garantiaid }) : RedirectToAction("List");
                }
                return View(data);
            }
            else
            {
                try
                {
                    Garantia dataDelete = db.Garantia.Find(data.garantiaid);
                    db.Garantia.Remove(dataDelete);
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
                    return RedirectToAction("Edit", new { id = data.garantiaid });
                }
                return RedirectToAction("List");
            }
        }

        public int RetornaDocGerado(int cod_garantia)
        {
            int retorno = 0;


            SacGarantia garantia = db.SacGarantia.Where(p => p.garantiaId == cod_garantia).FirstOrDefault();
            if (garantia != null)
            {
                retorno = garantia.cod_sac;
            }

            //int doc = procedimento.cod_procedimento == null ? 0 : procedimento.cod_procedimento;

            return retorno;
        }

        public ActionResult Details(int id)
        {
            string ids = id.ToString();
            ViewBag.comentarios = db.ListaComentarios.Where(a => a.cod_interno == ids && a.tipo_nota == "GARANTIA").ToList();
            ViewBag.troca_estagio = db.Garantia_Troca_Status.Where(a => a.garantiaId == id).OrderBy(p => p.num_seq).ToList();
            ViewBag.itens = db.GarantiaItem.Where(a => a.garantiaid == id).ToList();
            Garantia data = db.Garantia.Find(id);
            bool _ind_finalizada = data.GE_Status_Garantia.ind_finalizacao == "S";
            ViewBag.cod_gerado = RetornaDocGerado(id);



            ViewBag.cod_cliente = new SelectList(db.Ps_Pessoas, "cod_pessoa", "des_pessoa", data.cod_cliente);
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes, "cod_pessoa", "des_pessoa", data.cod_representante);
            ViewBag.cod_status = new SelectList(db.GE_Status_Garantia, "cod_status", "des_nome", data.cod_status);




            if (data == null)
            {
                return InvokeHttpNotFound();
            }

            return View(data);


        }

        public ActionResult ShowRecebimento(int id)
        {
            int TotalDeItens = db.GarantiaItem.Where(a => a.garantiaid == id).Count();
            int TotalVerificado = db.GarantiaItem.Where(a => a.garantiaid == id).Where(c => c.conferido == "S").Count();
            int[] data = new int[] { id, TotalDeItens, TotalVerificado };
            return View(data);

        }


        public ActionResult ReadItemRecebidos(ParametersDataTable param, int id)
        {
            //int total_de_linhas = db.IE_Itens_Vendas.AsNoTracking().Where(a => a.cod_cliente == id).Count();
            var allItem = db.GarantiaItem.Where(a => a.garantiaid == id);


            IEnumerable<GarantiaItem> filteredCompanies;

            if ((!string.IsNullOrEmpty(param.sSearch)))
            {
                filteredCompanies =
                    allItem.Where(c => c.IE_Itens.des_item.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            else
            {
                filteredCompanies = allItem;
            }


            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength).ToList();

            var result = from c in displayedCompanies
                         select new
                         {
                             caminho_foto = c.caminho_foto,
                             cod_item = c.cod_item,
                             Garantia = c.Garantia,
                             garantiaid = c.garantiaid,
                             garantiaitemid = c.garantiaitemid,
                             IE_Itens = c.IE_Itens,
                             num_nota = c.num_nota,
                             obs = c.obs,
                             qtd_avariada = c.qtd_avariada,
                             qtd_faltante = c.qtd_faltante,
                             qtd_outras_marcas = c.qtd_outras_marcas,
                             qtd_atendida = c.qtd_atendida,
                             qtd_descartada = c.qtd_descartada,
                             qtd_fora_garantia = c.qtd_fora_garantia,
                             qtd_lancamento = c.qtd_lancamento,
                             qtd_reaproveitada = c.qtd_reaproveitada,
                             qtd_recebida = c.qtd_recebida,
                             qtd_rejeitada = c.qtd_rejeitada,
                             vlr_lancamento = c.vlr_lancamento.ToString("c"),
                             vlr_total = c.vlr_total.ToString("c"),
                             Check = c.conferido == "S",
                             Img = "",
                             asError = ((c.qtd_atendida + c.qtd_fora_garantia + c.qtd_outras_marcas + c.qtd_faltante + c.qtd_avariada + c.qtd_descartada) != c.qtd_recebida) ? "error_input" : "ok_input"


                         };

            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allItem.Count(),
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;
        }

        public ActionResult Resposta(int id)
        {
            Garantia_Resposta_Model data = new Garantia_Resposta_Model { Obs = "", Garantia = db.Garantia.Find(id), num_nf_cliente = db.Garantia.Where(a => a.garantiaid == id).Select(a => a.num_nf_cliente).FirstOrDefault() };
            bool _ind_finalizada = data.Garantia.GE_Status_Garantia != null ? data.Garantia.GE_Status_Garantia.ind_finalizacao == "S" : false;

            if (_ind_finalizada)
            { return RedirectToAction("Details", new { id = id }); }


            if (data.Garantia == null)
            {
                return InvokeHttpNotFound();
            }

            ViewBag.cod_cliente = new SelectList(db.Ps_Pessoas, "cod_pessoa", "des_pessoa", data.Garantia.cod_cliente);
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes, "cod_pessoa", "des_pessoa", data.Garantia.cod_representante);
            ViewBag.cod_status = new SelectList(db.GE_Status_Garantia, "cod_status", "des_nome", data.Garantia.cod_status);
            ViewBag.troca_estagio = db.Garantia_Troca_Status.Where(a => a.garantiaId == id).OrderBy(p => p.num_seq).ToList();
            return View(data);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-continue", "continueAdd")]
        public ActionResult Resposta(Garantia_Resposta_Model data, int? cod_status, bool continueAdd)
        {
            ViewBag.cod_cliente = new SelectList(db.Ps_Pessoas, "cod_pessoa", "des_pessoa", data.Garantia.cod_cliente);
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes, "cod_pessoa", "des_pessoa", data.Garantia.cod_representante);
            ViewBag.cod_status = new SelectList(db.GE_Status_Garantia, "cod_status", "des_nome", data.Garantia.cod_status);
            ViewBag.troca_estagio = db.Garantia_Troca_Status.Where(a => a.garantiaId == data.Garantia.garantiaid).OrderBy(p => p.num_seq).ToList();


            Garantia data_update = db.Garantia.Find(data.Garantia.garantiaid);
            if (data_update == null) { return View(data); }



            if (ModelState.IsValid)
            {
                int? cod_status_atual = data_update.cod_status;
                string indica = db.GE_Status_Garantia.Where(a => a.Cod_Status == cod_status).Select(a => a.ind_finalizacao).FirstOrDefault();
                bool indica_estagio_final = indica == "S";


                if (indica_estagio_final)
                {
                    data_update.dta_finalizacao = System.DateTime.Now;
                }


                Garantia_Troca_Status altera_linha = AddDataSaidaEstagioAnterior(data_update.garantiaid);
                Garantia_Troca_Status nova_linha = AddNovaLinhaTrocaEstagio(data_update.garantiaid, data_update.cod_status, cod_status, data.Obs);


                data_update.cod_status = cod_status.HasValue ? (int)cod_status : data_update.cod_status;
                data_update.num_nf_cliente = data.num_nf_cliente;



                db.Entry(data_update).State = EntityState.Modified;
                db.Entry(altera_linha).State = EntityState.Modified;
                db.Garantia_Troca_Status.Add(nova_linha);

                db.SaveChanges();
                return continueAdd ? RedirectToAction("Resposta", new { id = data_update.garantiaid }) : RedirectToAction("Details", new { id = data_update.garantiaid });

            }

            return View(data);
        }


        public ActionResult GetNotas(int garantiaId)
        {
            var result = (from o in db.GarantiaItem
                          where o.garantiaid == garantiaId
                          group o by new { o.garantiaid, o.num_nota } into g
                          select new { Garantiaid = g.Key.garantiaid, Nota = g.Key.num_nota }).ToList();


            return Json(result);

        }

        public ActionResult FilterItemPrint(int id)
        {
            string ssql = string.Format(" DELETE FROM CartItemPrint WHERE GarantiaId = {0} ", id);
            db.Database.ExecuteSqlCommand(ssql);

            var result = db.GarantiaItem.Where(a => a.garantiaid == id).ToList();
            return View(result);
        }




        private string RenderViewToString(string viewName, object viewData)
        {
            var renderedView = new StringBuilder();

            var controller = this;


            using (var responseWriter = new StringWriter(renderedView))
            {
                var fakeResponse = new HttpResponse(responseWriter);
                var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request, fakeResponse);

                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(fakeContext), controller.ControllerContext.RouteData,
                    controller.ControllerContext.Controller);

                var oldContext = System.Web.HttpContext.Current;
                System.Web.HttpContext.Current = fakeContext;

                using (var viewPage = new ViewPage())
                {
                    var viewContext = new ViewContext(fakeControllerContext, new FakeView(), new ViewDataDictionary(), new TempDataDictionary(), responseWriter);

                    var html = new HtmlHelper(viewContext, viewPage);
                    html.RenderPartial(viewName, viewData);
                    System.Web.HttpContext.Current = oldContext;
                }
            }

            return renderedView.ToString();
        }
        public class FakeView : IView
        {
            #region IView Members

            public void Render(ViewContext viewContext, TextWriter writer)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public ActionResult AddOrRemove(int garantiaId, string cod_foxlux, int cod_item, bool isDelete, int num_nota)
        {
            int NextRecordId = db.Database.SqlQuery<int>(" SELECT CartItemPrintSeq.NextVal FROM DUAL ").FirstOrDefault();
            string Msg = "";


            if (!isDelete)
            {
                CartItemPrint NewItem = new CartItemPrint()
                {
                    garantiaId = garantiaId,
                    cod_Foxlux = cod_foxlux,
                    cod_item = cod_item,
                    num_nota = num_nota,
                    recordId = NextRecordId
                };
                db.CartItemPrint.Add(NewItem);
            }
            else
            {
                int RecordId = db.CartItemPrint.Where(a => a.cod_Foxlux == cod_foxlux && a.garantiaId == garantiaId && a.num_nota == num_nota).Select(a => a.recordId).FirstOrDefault();
                CartItemPrint DeleteItem = db.CartItemPrint.Find(garantiaId, RecordId, cod_foxlux, cod_item, num_nota);

                if (DeleteItem != null)
                {
                    db.CartItemPrint.Remove(DeleteItem);
                }
                else
                {
                    Msg = " Não existem Item para deletar";
                }
            }

            try
            {
                db.SaveChanges();
                Msg = " Item atualizado com êxito ";

            }
            catch (Exception e)
            {
                Msg = e.Message;
            }


            var _itens = db.CartItemPrint.Where(a => a.garantiaId == garantiaId).ToList();
            var results = new
            {
                Msg = Msg,
                CartCount = _itens.Count()
            };
            return Json(results);

        }



        [HttpPost]
        public ActionResult AddOrRemoveGarantiaImpressao(int garantiaId, bool isDelete)
        {
            var data = db.Tmp_Garantia_Impressao.Where(a => a.cod_usuario == cd_usuario && a.garantiaid == garantiaId).FirstOrDefault();

            if (isDelete)
            {
                if (data != null)
                {
                    db.Tmp_Garantia_Impressao.Remove(data);
                    db.SaveChanges();
                }

                return Json(new
                {
                    Msg = "",
                    CartCount = db.Tmp_Garantia_Impressao.Where(a => a.cod_usuario == cd_usuario).Count()
                });
            }

            if (data == null)
            {
                var novo = new Tmp_Garantia_Impressao()
                {
                    cod_usuario = cd_usuario,
                    garantiaid = garantiaId
                };
                db.Tmp_Garantia_Impressao.Add(novo);
                db.SaveChanges();
            }

            var results = new
            {
                Msg = "",
                CartCount = db.Tmp_Garantia_Impressao.Where(a => a.cod_usuario == cd_usuario).Count()
            };
            return Json(results);



        }


        [HttpPost]
        public ActionResult AddOrRemoveRegistraColeta(int garantiaId, bool isDelete)
        {
            var data = db.Garantia.Where(a => a.garantiaid == garantiaId).FirstOrDefault();



            if (data != null)
                data.Ind_Coletado = isDelete ? 0 : 1;


            bool hasError = false;
            string msg = "Registro alterado com sucesso";

            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                msg = e.Message;
                hasError = true;
            }

            var results = new
            {
                msg = msg,
                error = hasError
            };



            return Json(results);



        }


        public ActionResult ImpressaoNFDTodas(int codigoRepresentante)
        {
            List<Garantia> listaGarantias;


            int qt = db.Tmp_Garantia_Impressao.Where(a => a.cod_usuario == cd_usuario).Count();
            if (qt == 0)
            {
                listaGarantias = (from gat in db.Garantia
                                  where gat.cod_representante == codigoRepresentante
                                    && gat.ind_emitido_nf == "S"
                                    && gat.ind_emitido_coleta == "S"
                                    && gat.ind_cancelada == "N"
                                    && gat.dta_finalizacao == null
                                  select gat).ToList();
            }
            else
            {
                listaGarantias = (from gat in db.Garantia
                                  join gatim in db.Tmp_Garantia_Impressao on gat.garantiaid equals gatim.garantiaid
                                  where gatim.cod_usuario == cd_usuario
                                    && gat.cod_representante == codigoRepresentante
                                    && gat.ind_emitido_nf == "S"
                                    && gat.ind_emitido_coleta == "S"
                                    && gat.ind_cancelada == "N"
                                    && gat.dta_finalizacao == null
                                  select gat).ToList();
            }



            listaGarantias.ForEach(del => db.Database.ExecuteSqlCommand(string.Format(" DELETE FROM CartItemPrint WHERE GarantiaId = {0} ", del.garantiaid)));


            //cod_representante
            //string ssql = string.Format(" DELETE FROM CartItemPrint WHERE GarantiaId = {0} ", cod_garantia_print);
            //db.Database.ExecuteSqlCommand(ssql);


            string caminho = GeraXML(0, "", 0, 0, false, true, listaGarantias);

            var model = new DanzorPrintViewer(caminho);


            string htmlText = RenderViewToString("~/Views/Danfe/Danfe.cshtml", model);
            htmlText = htmlText.Replace("\r", "");
            htmlText = htmlText.Replace("\n", "");
            htmlText = htmlText.Replace("\"", "\'");

            htmlText = htmlText.Replace("<head>", "<head> <link href='http://sac.grupofoxlux.com.br/Content/css/danfe/danfe.min.css' rel='stylesheet' />");



            string NomeArquivo = "Impressão Garantia " + System.DateTime.Now;
            string contentype = "text/html";


            return View("~/Views/Danfe/Danfe.cshtml", model);



        }


        public ActionResult ImpressaoNFD(int cod_garantia_print, string ObsImpressao, int num_nota, bool registra_contabil, bool registra_impressao)
        {
            

            int nextt = 0;

            if (registra_impressao)
            {
                nextt = db.Database.SqlQuery<Int32>("select GarantiaArqSeq.NextVal from dual ").FirstOrDefault<Int32>();
            }


            string caminho = GeraXML(cod_garantia_print, ObsImpressao, num_nota, nextt, registra_contabil, false, null);

            var model = new DanzorPrintViewer(caminho);

            string ssql = string.Format(" DELETE FROM CartItemPrint WHERE GarantiaId = {0} ", cod_garantia_print);

            string htmlText = RenderViewToString("~/Views/Danfe/Danfe.cshtml", model);
            htmlText = htmlText.Replace("\r", "");
            htmlText = htmlText.Replace("\n", "");
            htmlText = htmlText.Replace("\"", "\'");

            htmlText = htmlText.Replace("<head>", "<head> <link href='http://sac.grupofoxlux.com.br/Content/css/danfe/danfe.min.css' rel='stylesheet' />");



            string NomeArquivo = "Impressão Garantia " + System.DateTime.Now;
            string contentype = "text/html";
            byte[] arqUp = Encoding.ASCII.GetBytes(htmlText);



            if (registra_impressao)
            {

                GarantiaArq cat = new GarantiaArq
                {
                    id = nextt,
                    garantiaid = cod_garantia_print,
                    caminho = NomeArquivo,
                    des_contenttype = contentype,
                    des_imagem = arqUp,
                    nome_arquivo = NomeArquivo
                };
                db.GarantiaArq.Add(cat);
                db.SaveChanges();
            }



            return View("~/Views/Danfe/Danfe.cshtml", model);



        }


        public int getCfop(string estado, decimal vlr_icms_sub)
        {
            int cd_cfop = 5411;

            if ("Paraná".Equals(estado))
            {
                cd_cfop = (vlr_icms_sub > 0 ? 5411 : 5202);// tam 4
            }
            else
            {
                cd_cfop = (vlr_icms_sub > 0 ? 6411 : 6202);// tam 4

            }

            return cd_cfop;
        }

        public string GeraXML(int id, string Obs, int num_nota, int idArquivo, bool registra_contabil, bool isRepresentante, List<Garantia> listaGarantias)
        {

            Obs += "Garantia número " + id.ToString("d8");
            //inserrir na
            //db.GarantiaNotaClienteImpressao;
            //db.GarantiaNotaClienteItem;

            int NotaClienteSeq = db.Database.SqlQuery<Int32>("select NotaClienteSeq.NextVal from dual ").FirstOrDefault<Int32>();
            string cod_pessoa;
            if (listaGarantias == null)
            {
                cod_pessoa = db.Garantia.Where(a => a.garantiaid == id).Select(p => p.cod_cliente).First().ToString();
            }
            else
            {
                cod_pessoa = listaGarantias[0].cod_representante.ToString();
            }



            var DadosDoCliente = db.Dados_crm.Where(a => a.cod_pessoa == cod_pessoa).First();
            IEnumerable<GarantiaItem> itens;
            int _cd_cfop = 5411;


            if (listaGarantias == null)
            {

                int qt = db.CartItemPrint.Where(a => a.garantiaId == id).Select(a => a.garantiaId).Count();
                if (qt == 0)
                {
                    itens = (from a in db.GarantiaItem.Where(a => a.garantiaid == id && a.num_nota == (num_nota > 0 ? num_nota : a.num_nota)) select a).ToList();
                    //db.GarantiaItem.Where(a => a.garantiaid == id && a.num_nota == (num_nota > 0 ? num_nota : a.num_nota)).ToList();
                }
                else
                {
                    itens = (from t1 in db.GarantiaItem
                             join t2 in db.CartItemPrint
                              on new { A = t1.cod_foxlux, B = t1.cod_item, C = t1.garantiaid } equals new { A = t2.cod_Foxlux, B = t2.cod_item, C = t2.garantiaId }
                             where t1.garantiaid == id && t1.num_nota == (num_nota > 0 ? num_nota : t1.num_nota)
                             select t1).ToList();
                }
            }
            else
            {
                int codrepresentante = listaGarantias[0].cod_representante;

                itens = (from gat in db.Garantia
                         join it in db.GarantiaItem
                         on new { a = gat.garantiaid } equals new { a = it.garantiaid }
                         where gat.cod_representante == codrepresentante
                         && gat.ind_emitido_nf == "S"
                         && gat.ind_emitido_coleta == "S"
                         && gat.ind_cancelada == "N"
                         && gat.dta_finalizacao == null
                         select it).ToList();
            }


            if (registra_contabil)
            {

                var notaCliente = new NotaCliente()
                {
                    DataImpressao = System.DateTime.Now,
                    GarantiaId = id,
                    Id = NotaClienteSeq,
                    NotasSelecionadas = num_nota.ToString(),
                    Obs = Obs,
                    UsuarioId = cd_usuario,
                    ArquivoId = idArquivo,
                    num_nota = 0,
                    DataEntrada = null,
                    Vlr_mercadorias = 0,
                    Vlr_Nota = 0
                };
                db.NotaCliente.Add(notaCliente);

                List<string> num_notas = new List<string>();

                num_notas.Add(notaCliente.NotasSelecionadas);

                foreach (var item in itens)
                {
                    int NotaItemClienteSeq = db.Database.SqlQuery<Int32>("select NotaItemClienteSeq.NextVal from dual ").FirstOrDefault<Int32>();
                    var notaItemCliente = new NotaItemCliente()
                    {
                        Cod_Foxlux = item.cod_foxlux,
                        GarantiaItemId = item.garantiaitemid,
                        Id = NotaItemClienteSeq,
                        NotaClienteId = NotaClienteSeq,
                        MVast = item.mvast,
                        Per_Icms = item.picms,
                        Per_IcmsSt = item.picmsst,
                        Per_Ipi = item.pipi,
                        Qtd_Lancamento = item.qtd_lancamento,
                        Vlr_Base_Subs = item.vlr_base_subs,
                        Vlr_icms = item.vlr_icms,
                        Vlr_Imcs_Subs = item.vlr_icms_subs,
                        Vlr_ipi = item.vlr_ipi,
                        Vlr_Lancamento = item.vlr_lancamento,
                        Vlr_Total = item.vlr_total,
                        cd_cfop = getCfop(DadosDoCliente.des_estado, item.vlr_base_subs)
                    };
                    notaCliente.Vlr_mercadorias += item.vlr_total;
                    notaCliente.Vlr_Nota += item.vlr_total;
                    num_notas.Add(item.num_nota.ToString());

                    db.NotaItemCliente.Add(notaItemCliente);
                }
                notaCliente.NotasSelecionadas = string.Join(",", (new HashSet<string>(num_notas)).ToArray());

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Response.Write("Erro ao Gerar Histórico de NF" + ex.ToString());
                }

            }







            //db.GarantiaItem.Where(a => a.garantiaid == id && a.num_nota == (num_nota > 0 ? num_nota : a.num_nota)).ToList();


            var DadosGarantia = new Garantia();
            if (listaGarantias == null)
            {
                DadosGarantia = db.Garantia.Where(a => a.garantiaid == id).First();
            }
            else
            {
                int garantiaId = listaGarantias[0].garantiaid;
                DadosGarantia = db.Garantia.Where(a => a.garantiaid == garantiaId).First();

            }



            //var DadosTrans = db.Garantia.Where(a => a.garantiaid == id).Select(a => a.Transportador.FANTASIA).FirstOrDefault();


            string caminho = Path.Combine(HttpContext.Server.MapPath(string.Format("~/Content/XmlDeNotas/{0}.xml", Guid.NewGuid())));
            if (System.IO.File.Exists(caminho))
            {
                System.IO.File.Delete(caminho);
            }



            try
            {
                UTF8Encoding _utf8 = new UTF8Encoding();
                XmlTextWriter _xml = new XmlTextWriter(caminho, _utf8);
                _xml.WriteStartDocument();


                #region enviNFe


                _xml.WriteStartElement("enviNFe");
                _xml.WriteStartAttribute("xmlns");
                _xml.WriteString("http://www.portalfiscal.inf.br/nfe");
                _xml.WriteEndAttribute();
                _xml.WriteStartAttribute("versao");// escrevendo no atributo
                _xml.WriteString("3.10");
                _xml.WriteEndAttribute();

                //_xml.WriteStartAttribute("xmlns:xsi");
                //_xml.WriteString("http://www.w3.org/2001/XMLSchema-instance");// escrevendo no atributo
                //_xml.WriteEndAttribute();
                //_xml.WriteStartAttribute("xsi:schemaLocation");
                //_xml.WriteString("http://www.portalfiscal.inf.br/nfe enviNFe_v2.00.xsd");// escrevendo no atributo
                //_xml.WriteEndAttribute();

                _xml.WriteStartElement("idLote");//Tag Lote, seguencial controlando no sistema.
                _xml.WriteString(id.ToString());// numero do lote
                _xml.WriteEndElement();

                _xml.WriteStartElement("indSinc");
                _xml.WriteString("0");
                _xml.WriteEndElement();


                #region NFe
                _xml.WriteStartElement("NFe", "http://www.portalfiscal.inf.br/nfe");// TAG raiz da NF-e, Nó com tipo de

                _xml.WriteStartAttribute("xmlns");
                _xml.WriteString("http://www.portalfiscal.inf.br/nfe");
                _xml.WriteEndAttribute();




                #region infNFe
                _xml.WriteStartElement("infNFe");//Grupo que contém as informações da NF-e;

                _xml.WriteStartAttribute("versao");////Versão do leiaute (v2.0),Atributos do Nó
                _xml.WriteString("3.10");//vesao da nfe.
                _xml.WriteEndAttribute();



                _xml.WriteStartAttribute("Id");// informar a chave de acesso da
                                               //NF-e precedida do literal ‘NFe’,
                                               //acrescentada a validação do
                                               //formato (v2.0).
                _xml.WriteString("Nfe"
                    + "41"
                    + System.DateTime.Now.ToString("yy")
                    + System.DateTime.Now.Month.ToString("d2")
                    + DadosDoCliente.cgc_cpf
                    + "02"
                    + "001"
                    + id.ToString("d9")
                    + "01"
                    + id.ToString("d9"));//160908924247000106550010000042871000042879");
                _xml.WriteEndAttribute();



                #region ide
                _xml.WriteStartElement("ide");//ide (inserida como filha na tag infNFe)
                _xml.WriteStartElement("cUF");//Código da UF do emitente do Documento Fiscal. Utilizar a Tabela do IBGE de código de unidades da federação (Anexo IV - Tabela de UF, Município e País).
                _xml.WriteString("41");// Código da Uf, tam. 2
                _xml.WriteEndElement();

                _xml.WriteStartElement("cNF");//Código numérico que compõe a Chave de Acesso. Número aleatório gerado pelo emitente para cada NF-e para evitar acessos indevidos da NF-e.(v2.0)
                _xml.WriteString(num_nota.ToString("d8"));// Código da Nf, tam. 8
                _xml.WriteEndElement();

                _xml.WriteStartElement("natOp");//Descrição da Natureza da Operação
                _xml.WriteString("DEVOLUCAO MERCADORIAS EM GARANTIA");// Tam. 1-60
                _xml.WriteEndElement();

                _xml.WriteStartElement("indPag");//Indicador forma de pagamento; 0 – pagamento à vista;1 – pagamento à prazo;2 - outros.
                _xml.WriteString("1");// tam. 1
                _xml.WriteEndElement();

                _xml.WriteStartElement("mod");//Código do Modelo do Documento Fiscal
                _xml.WriteString("55");// tam. 2; Utilizar o código 55 para identificação da NF-e, emitida em substituição ao modelo 1 ou 1A.
                _xml.WriteEndElement();

                _xml.WriteStartElement("serie");//Série do Documento Fiscal
                _xml.WriteString("1");// Tam 1-3
                _xml.WriteEndElement();

                _xml.WriteStartElement("nNF");//Número do Documento Fiscal
                _xml.WriteString(id.ToString());// Tam. 1 - 9
                _xml.WriteEndElement();

                _xml.WriteStartElement("dEmi");//Data de emissão do Documento Fiscal
                _xml.WriteString(System.DateTime.Now.ToString("yyyy-MM-dd"));// Formato “AAAA-MM-DD”
                _xml.WriteEndElement();

                _xml.WriteStartElement("dSaiEnt");//Data de Saída ou da Entrada da Mercadoria/Produto
                _xml.WriteString(System.DateTime.Now.ToString("yyyy-MM-dd"));// Formato “AAAA-MM-DD”
                _xml.WriteEndElement();

                //ocorrencia 0-1
                _xml.WriteStartElement("hSaiEnt");//Hora de Saída ou da Entrada da Mercadoria/Produto
                _xml.WriteString("00:00:00");// Formato “HH:MM:SS” (v.2.0)
                _xml.WriteEndElement();

                _xml.WriteStartElement("tpNF");//Tipo de Operação
                _xml.WriteString("1");// 0-entrada / 1-saída, tam. 1
                _xml.WriteEndElement();

                _xml.WriteStartElement("cMunFG");//Código do Município de Ocorrência do Fato Gerador
                _xml.WriteString("4106902");// tam. 7
                _xml.WriteEndElement();


                if (1 == 2)
                {
                    //ide (inserida como filha na tag infNFe)//Grupo com as informações das NF/NF-e /NF de produtor/Cupom Fiscal referenciadas.
                    //Esta informação será utilizada nas hipóteses previstas na legislação. (Ex.: Devolução de Mercadorias, Substituição de NF cancelada, Complementação de NF, etc.). (v.2.0)
                    //Ocorrencia 0-N B12a
                    #region NFref
                    _xml.WriteStartElement("NFref");

                    _xml.WriteStartElement("refNFe");//Chave de acesso da NF-e referenciada
                    _xml.WriteString("1");// Tam 44, Utilizar esta TAG para referenciar uma Nota Fiscal Eletrônica emitida anteriormente, vinculada a NF-e atual.
                    _xml.WriteEndElement();

                    #region refNF
                    _xml.WriteStartElement("refNF");//Grupo de informação da NF modelo 1/1A referenciada

                    _xml.WriteStartElement("cUF");//Código da UF do emitente do Documento Fiscal
                    _xml.WriteString("1");// tam. 2
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("AAMM");//Ano e Mês de emissão da NFe
                    _xml.WriteString("1");// tam. 4
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("CNPJ");//CNPJ do emitente
                    _xml.WriteString("1");// tam. 14
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("mod");//Modelo do Documento Fiscal
                    _xml.WriteString("1");// tam. 2
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("serie");//Série do Documento Fiscal
                    _xml.WriteString("1");// tam. 1-3
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("nNF");//Número do Documento Fiscal
                    _xml.WriteString("1");// tam. 1-9
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();
                    #endregion

                    //ou
                    #region refNFP
                    _xml.WriteStartElement("refNFP");//Grupo de informação da NF modelo 1/1A referenciada

                    //Grupo de informações da NF de produtor rural referenciada
                    _xml.WriteStartElement("cUF");//Código da UF do emitente do Documento Fiscal
                    _xml.WriteString("1");// tam. 2
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("AAMM");//Ano e Mês de emissão da NFe
                    _xml.WriteString("1");// tam. 4
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("CNPJ");//Informar o CNPJ do emitente da NF de produtor (v2.0)
                    _xml.WriteString("1");// tam. 14
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("CPF");//Informar o CPF do emitente da NF de produtor (v2.0)
                    _xml.WriteString("1");// tam. 11
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("IE");//Informar a IE do emitente da NF de Produtor (v2.0)
                    _xml.WriteString("1");// tam. 1-14
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("mod");//Modelo do Documento Fiscal
                    _xml.WriteString("1");// tam. 2
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("serie");//Série do Documento Fiscal
                    _xml.WriteString("1");// tam. 1-3
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("nNF");//Número do Documento Fiscal
                    _xml.WriteString("1");// tam. 1-9
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();
                    #endregion

                    _xml.WriteStartElement("refCTe");//Chave de acesso do CT-e referenciada
                    _xml.WriteString("1");// tam. 44
                    _xml.WriteEndElement();

                    #region refECF
                    //Informações do Cupom Fiscal referenciado, vinculado à NF-e (v2.0).
                    _xml.WriteStartElement("refECF");

                    _xml.WriteStartElement("mod");//Preencher com "2B", quando se tratar de Cupom Fiscal emitido
                                                  //por máquina registradora (não ECF), com "2C", quando se tratar de Cupom Fiscal PDV, ou "2D",
                                                  //quando se tratar de Cupom Fiscal (emitido por ECF) (v2.0).
                    _xml.WriteString("1");// tam. 2
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("nECF");//Número de ordem seqüencial do ECF
                    _xml.WriteString("1");// tam. 3
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("nCOO");//Número do Contador de Ordem de Operação - COO
                    _xml.WriteString("1");// tam. 6
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();
                    #endregion

                    _xml.WriteEndElement();

                    #endregion

                }

                _xml.WriteStartElement("tpImp");//Formato de Impressão do DANFE
                _xml.WriteString("1");//1-Retrato/ 2-Paisagem
                _xml.WriteEndElement();

                _xml.WriteStartElement("tpEmis");//Tipo de Emissão da NF-e (normal, contingencia, ver manual)
                _xml.WriteString("1");// tam. 1
                _xml.WriteEndElement();

                _xml.WriteStartElement("cDV");//Dígito Verificador da Chave de Acesso da NF-e
                _xml.WriteString("4");// tam. 1
                _xml.WriteEndElement();

                _xml.WriteStartElement("tpAmb");//Tipo de ambiente,1-Produção/ 2-Homologação
                _xml.WriteString("2");// Tam 1
                _xml.WriteEndElement();

                _xml.WriteStartElement("finNFe");//Finalidade de emissão da NFe,1- NF-e normal/ 2-NF-e complementar / 3 – NF-e de ajuste
                _xml.WriteString("1");// Finalidade da Nfe
                _xml.WriteEndElement();

                _xml.WriteStartElement("procEmi");//Processo de emissão da NF-e
                _xml.WriteString("1");// Tam 1
                _xml.WriteEndElement();

                _xml.WriteStartElement("verProc");//Versão do Processo de emissão da NF-e
                _xml.WriteString("1");// tam. 1-20
                _xml.WriteEndElement();

                _xml.WriteEndElement();//ide
                                       //fim ide
                #endregion
                //omitido tags dhCont, xJust
                #region emit


                _xml.WriteStartElement("emit");//ide

                if (DadosDoCliente.cgc_cpf.Length == 14)
                {
                    _xml.WriteStartElement("CNPJ");// CNPJ Emitente
                    _xml.WriteString(Convert.ToUInt64(DadosDoCliente.cgc_cpf).ToString(@"00\.000\.000\/0000\-00"));// tam 14
                    _xml.WriteEndElement();
                    //or
                }
                else
                {
                    _xml.WriteStartElement("CPF");//CPF Emitente
                    _xml.WriteString(Convert.ToUInt64(DadosDoCliente.cgc_cpf).ToString(@"000\.000\.000\-00"));// Tam. 11
                    _xml.WriteEndElement();
                }





                _xml.WriteStartElement("xNome");//Razão Social ou Nome do emitente
                _xml.WriteString(DadosDoCliente.des_pessoa);// tam. 2-60
                _xml.WriteEndElement();

                _xml.WriteStartElement("xFant");//Nome fantasia
                _xml.WriteString(DadosDoCliente.des_fantasia);// tam. 1-60
                _xml.WriteEndElement();
                #region enderEmit
                _xml.WriteStartElement("enderEmit");//Tag codigo Uf

                _xml.WriteStartElement("xLgr");//Logradouro
                _xml.WriteString(DadosDoCliente.des_endereco);//tam. 2-60
                _xml.WriteEndElement();

                _xml.WriteStartElement("nro");//Número
                _xml.WriteString(String.Join("", System.Text.RegularExpressions.Regex.Split(DadosDoCliente.des_endereco, @"[^\d]")));// 1-60
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("xCpl");//Número
                _xml.WriteString("");// 1-60
                _xml.WriteEndElement();

                _xml.WriteStartElement("xBairro");//Bairro
                _xml.WriteString(DadosDoCliente.bairro);// tam. 2-60
                _xml.WriteEndElement();

                _xml.WriteStartElement("cMun");//Código do município
                _xml.WriteString("4106902");// 1
                _xml.WriteEndElement();

                _xml.WriteStartElement("xMun");//Nome do município
                _xml.WriteString(DadosDoCliente.des_cidade);// tam. 2-60
                _xml.WriteEndElement();

                _xml.WriteStartElement("UF");//Sigla da UF
                _xml.WriteString(DadosDoCliente.des_estado);// tam. 2
                _xml.WriteEndElement();

                _xml.WriteStartElement("CEP");//Código do CEP, Informar os zeros não significativos.
                _xml.WriteString(Convert.ToUInt64(DadosDoCliente.cep).ToString(@"00\.000\-000"));// tam. 8
                _xml.WriteEndElement();

                _xml.WriteStartElement("cPais");//Código do País
                _xml.WriteString("1058");// tam. 4 1058 - Brasil
                _xml.WriteEndElement();

                _xml.WriteStartElement("xPais");//Nome do País
                _xml.WriteString("Brasil");// tam. 1-60; Brasil ou BRASIL
                _xml.WriteEndElement();

                _xml.WriteStartElement("fone");//Telefone
                _xml.WriteString(DadosDoCliente.tel_contato);// tam. 6-14 - Preencher com o Código DDD + número do telefone.
                                                             //Nas operações com exterior é permitido informar o código do país + código da localidade + número do telefone (v.2.0)
                _xml.WriteEndElement();
                _xml.WriteEndElement();//enderEmit
                #endregion

                _xml.WriteStartElement("IE");//IE
                _xml.WriteString(DadosDoCliente.ie);// tam. 0-14
                _xml.WriteEndElement();
                //Ocorrencia 0-1
                _xml.WriteStartElement("IEST");//IE do Substituto Tributário
                _xml.WriteString("");// tam. 2-14
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("IM");//Inscrição Municipal
                _xml.WriteString("");// tam. 1-15
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("CNAE");//CNAE fiscal
                _xml.WriteString("");// tam. 7
                _xml.WriteEndElement();

                _xml.WriteStartElement("CRT");//Código de Regime Tributário
                                              //Este campo será obrigatoriamente preenchido com:
                                              //1 – Simples Nacional;
                                              //2 – Simples Nacional – excesso de sublimite de receita bruta;
                                              //3 – Regime Normal. (v2.0).
                _xml.WriteString("1");// Tam. 1
                _xml.WriteEndElement();
                _xml.WriteEndElement();//emit
                #endregion
                //omitida tags avulsa e filhos
                #region dest
                _xml.WriteStartElement("dest");//dest

                _xml.WriteStartElement("CNPJ");//CNPJ do destinatário

                if (DadosDoCliente.cod_gerente >= 4000 && DadosDoCliente.cod_gerente <= 4999)
                {
                    _xml.WriteString("01.723.086/0005-77");// tam. 0-14
                }
                else { 
                    _xml.WriteString("01.723.086/0001-43");// tam. 0-14
                }

                _xml.WriteEndElement();
                //or
                //_xml.WriteStartElement("CPF");//CPF do destinatário
                //_xml.WriteString("13955471000103");// tam. 11
                //_xml.WriteEndElement();

                _xml.WriteStartElement("xNome");//Razão Social ou nome do destinatário
                _xml.WriteString("FOXLUX SA");// tam 2-60
                _xml.WriteEndElement();

                #region enderDest

                if (DadosDoCliente.cod_gerente >= 4000 && DadosDoCliente.cod_gerente <= 4999)
                {
                    _xml.WriteStartElement("enderDest");//enderDest

                    _xml.WriteStartElement("xLgr");//Logradouro
                    _xml.WriteString("RODOVIA BR 101 - SUL, S/N KM 80,55 GALPAO B2");// tam. 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("nro");//Número
                    _xml.WriteString("S/N");// 1-60
                    _xml.WriteEndElement();
                    //Ocorrencia 0-1
                    _xml.WriteStartElement("xCpl");//Complemento
                    _xml.WriteString("KM 80,55 GALPAO B2");// tam. 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xBairro");//Bairro
                    _xml.WriteString("JARDIM JORDÃO");//tam 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cMun");//Código do município
                    _xml.WriteString("4119152");//tam 7
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xMun");//Nome do município
                    _xml.WriteString("JABOATAO DOS GUARARAPES");//tam 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("UF");//Sigla da UF
                    _xml.WriteString("PE");// tam. 2
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("CEP");//Código do CEP, Informar os zeros não significativos.
                    _xml.WriteString("54.320-230");// tam. 8
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cPais");//Código do País
                    _xml.WriteString("1058");// tam 2-4
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xPais");//Nome do País
                    _xml.WriteString("Brasil");// tam. 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("fone");//Telefone
                    _xml.WriteString("41 3302-8100");//6-14
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();//enderDest
                }
                else
                {

                    _xml.WriteStartElement("enderDest");//enderDest

                    _xml.WriteStartElement("xLgr");//Logradouro
                    _xml.WriteString("RUA SANTA HELENA");// tam. 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("nro");//Número
                    _xml.WriteString("894");// 1-60
                    _xml.WriteEndElement();
                    //Ocorrencia 0-1
                    _xml.WriteStartElement("xCpl");//Complemento
                    _xml.WriteString("");// tam. 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xBairro");//Bairro
                    _xml.WriteString("EMILIANO PERNETA");//tam 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cMun");//Código do município
                    _xml.WriteString("4119152");//tam 7
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xMun");//Nome do município
                    _xml.WriteString("PINHAIS");//tam 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("UF");//Sigla da UF
                    _xml.WriteString("PR");// tam. 2
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("CEP");//Código do CEP, Informar os zeros não significativos.
                    _xml.WriteString("83.324-325");// tam. 8
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cPais");//Código do País
                    _xml.WriteString("1058");// tam 2-4
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xPais");//Nome do País
                    _xml.WriteString("Brasil");// tam. 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("fone");//Telefone
                    _xml.WriteString("41 3302-8100");//6-14
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();//enderDest
                }
                #endregion

                _xml.WriteStartElement("IE");//IE
                _xml.WriteString("901.28070-39");// tam 0,2-14
                _xml.WriteEndElement();
                //omitido ISUF
                _xml.WriteStartElement("email");//email - Informar o e-mail do destinatário.
                _xml.WriteString("foxlux@foxlux.com.br");// tam 1-60
                _xml.WriteEndElement();

                _xml.WriteEndElement();//dest
                #endregion
                //Omitida tags retirada e filhos

                if (1 == 2)
                {
                    #region entrega
                    _xml.WriteStartElement("entrega");//Tag codigo Uf

                    _xml.WriteStartElement("CNPJ");//Informar o CNPJ ou o CPF,preenchendo os zeros não significativos. (v2.0)
                    _xml.WriteString("13955471000103");// tam. 0-14
                    _xml.WriteEndElement();
                    //or
                    _xml.WriteStartElement("CPF");//Informar o CNPJ ou o CPF,preenchendo os zeros não significativos. (v2.0)
                    _xml.WriteString("13955471000103");// tam. 11
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xLgr");//Logradouro
                    _xml.WriteString("RUA JOAO DE CARVALHO");// tam. 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("nro");//Número
                    _xml.WriteString("10");// tam. 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xCpl");//Complemento
                    _xml.WriteString("10");// tam. 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xBairro");//Bairro
                    _xml.WriteString("SE");// tam. 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cMun");//Código do município
                    _xml.WriteString("3500303");//tam 7
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xMun");//Nome do município
                    _xml.WriteString("Aguai");// tam 2-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("UF");//Sigla da UF
                    _xml.WriteString("SP");// tam 2
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();//entrega
                    #endregion

                }

                #region det


                //aqui jose
                int incrementoProd = 1;
                foreach (var item in itens)//Itens da Nota
                {

                    _xml.WriteStartElement("det");//Grupo do detalhamento de Produtos e Serviços da NF-e
                    _xml.WriteStartAttribute("nItem");//Número do item (1-990)
                    _xml.WriteString(incrementoProd.ToString());
                    _xml.WriteEndAttribute();// finalizando o atributo
                    #region prod
                    _xml.WriteStartElement("prod");//TAG de grupo do detalhamento de Produtos e Serviços da NF-e

                    _xml.WriteStartElement("cProd");//Código do produto ou serviço
                    _xml.WriteString(item.IE_Itens.cod_foxlux);// tam 1-60
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cEAN");//GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras
                    _xml.WriteString("");// tam 0,8,12,13,14
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("xProd");//Descrição do produto ou serviço
                    _xml.WriteString(item.IE_Itens.des_item + "<br />" + "NF: Origem: " + item.num_nota);// tam 1-120

                    _xml.WriteEndElement();

                    _xml.WriteStartElement("NCM");//Código NCM com 8 dígitos ou 2 dígitos (gênero)
                    _xml.WriteString(item.IE_Itens.ncm);// tam 2,8
                    _xml.WriteEndElement();

                    //omitida tag EX_TIPI
                    _xml.WriteStartElement("CFOP");//Código Fiscal de Operações e Prestações

                    if ("Paraná".Equals(DadosDoCliente.des_estado))
                    {
                        _xml.WriteString(item.vlr_icms_subs > 0 ? "5411" : "5202");// tam 4
                    }
                    else
                    {
                        _xml.WriteString(item.vlr_icms_subs > 0 ? "6411" : "6202");// tam 4

                    }



                    _xml.WriteEndElement();

                    _xml.WriteStartElement("uCom");//Unidade Comercial
                    _xml.WriteString("UNI");// tam 1-6
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("qCom");//Quantidade Comercial
                    _xml.WriteString(item.qtd_lancamento.ToString());//tam 15
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vUnCom");//Valor Unitário de Comercialização
                    _xml.WriteString(item.vlr_lancamento.ToString("f2"));//tam 21
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vProd");//Valor Total Bruto dos Produtos ou Serviços
                    _xml.WriteString(item.vlr_total.ToString("f2"));// tam 15
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cEANTrib");//GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras
                    _xml.WriteString("");// tam 0,8,12,13,14
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("uTrib");//Unidade Tributável
                    _xml.WriteString("UNI");// tam 1-6
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("qTrib");//Quantidade Tributável
                    _xml.WriteString(item.qtd_lancamento.ToString());//tam 15
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vUnTrib");//Valor Unitário de tributação
                    _xml.WriteString("");// tam 21
                    _xml.WriteEndElement();
                    //ocorrencia 0-1
                    _xml.WriteStartElement("vFrete");//Valor Total do Frete
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();
                    //ocorrencia 0-1
                    _xml.WriteStartElement("vSeg");//Valor Total do Seguro
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();
                    //ocorrencia 0-1
                    _xml.WriteStartElement("vDesc");//Valor do Desconto
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();
                    //ocorrencia 0-1
                    _xml.WriteStartElement("vOutro");//Outras despesas acessórias
                    _xml.WriteString("");// tam 15

                    _xml.WriteEndElement();

                    _xml.WriteStartElement("indTot");//Indica se valor do Item (vProd) entra no valor total da NF-e (vProd)
                    _xml.WriteString("1");// Este campo deverá ser preenchido com:
                                          //0 – o valor do item (vProd) não compõe o valor total da NF-e(vProd)
                                          //1 – o valor do item (vProd)compõe o valor total da NF-e(vProd) (v2.0)
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();//fim prod
                    #endregion

                    #region imposto
                    _xml.WriteStartElement("imposto");//Grupo de Tributos incidentes no Produto ou Serviço
                    #region ICMS
                    _xml.WriteStartElement("ICMS");//Grupo do ICMS da Operação própria e ST

                    #region ICMS_ValComum
                    _xml.WriteStartElement("ICMS00");// + "SN102");//Grupo do ICMS da Operação própria e ST

                    //para o caso ICMS00,10,20,30,40,51,90,Part,ST,101,102,201,202,500,900
                    _xml.WriteStartElement("orig");//Origem da mercadoria
                    _xml.WriteString("0");// tam 1
                    _xml.WriteEndElement();

                    //para o caso de ICMS101,102,201,202,500,900
                    //_xml.WriteStartElement("CSOSN");//Código de Situação da Operação – Simples Nacional
                    //_xml.WriteString("102");// tam 3
                    //_xml.WriteEndElement();

                    //para o caso ICMS00,10,20,30,40,51,90,Part,ST
                    _xml.WriteStartElement("CST");//Tributação do ICMS = 00 – Tributada integralmente
                    _xml.WriteString("00");// tam 2
                    _xml.WriteEndElement();



                    //para o caso ICMS60,ST,500
                    #region ICMS60_ST_500
                    //_xml.WriteStartElement("vBCSTRet");//Valor da BC do ICMS ST retido
                    //_xml.WriteString("4");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vICMSSTRet");//Valor do ICMS ST retido
                    //_xml.WriteString("00");// tam 15
                    //_xml.WriteEndElement();
                    #endregion

                    //para o caso ICMSST
                    #region ICMSST
                    //_xml.WriteStartElement("vBCSTDest");//Valor da BC do ICMS ST da UF destino
                    //_xml.WriteString("4");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vICMSSTDest");//Valor do ICMS ST da UF destino
                    //_xml.WriteString("00");// tam 15
                    //_xml.WriteEndElement();
                    #endregion

                    //para o caso ICMS00,10,20,51,70,90,Part,900
                    //_xml.WriteStartElement("modBC");//Modalidade de determinação da BC do ICMS
                    //_xml.WriteString("1");// tam 1
                    //_xml.WriteEndElement();
                    ////para o caso de ICMS20,51,70,90(obs: no manual vem depois do vBC)
                    #region ICMS20_ICMS51
                    //_xml.WriteStartElement("pRedBC");//Percentual da Redução de BC
                    //_xml.WriteString("43195");// tam 5
                    //_xml.WriteEndElement();
                    #endregion

                    //para o caso ICMS00,10,20,51,70,90,Part,900
                    _xml.WriteStartElement("vBC");//Valor da BC do ICMS
                    _xml.WriteString(item.vlr_total.ToString("f2"));// tam 15
                    _xml.WriteEndElement();

                    //para o caso de ICMS90,Part,900
                    #region ICMS90_Part_900
                    //_xml.WriteStartElement("pRedBC");//Percentual da Redução de BC
                    //_xml.WriteString("43195");// tam 5
                    //_xml.WriteEndElement();
                    #endregion

                    //para o caso ICMS00,10,20,40,51,70,90,Part,900
                    _xml.WriteStartElement("pICMS");//Alíquota do imposto
                    _xml.WriteString(item.picms.ToString());// tam 5
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vICMS");//Valor do ICMS
                    _xml.WriteString(item.vlr_icms.ToString("f2"));// tam 5
                    _xml.WriteEndElement();

                    //para o caso de ICMS10,30,70,90,Part,201,202,900
                    #region ICMS10_30_70_90_Part_201_202_900
                    _xml.WriteStartElement("modBCST");//Modalidade de determinação da BC do ICMS ST
                    _xml.WriteString("4");// tam 1
                    _xml.WriteEndElement();
                    //ocorrencia 0-1
                    _xml.WriteStartElement("pMVAST");//Percentual da margem de valor Adicionado do ICMS ST
                    _xml.WriteString("00");// tam 5
                    _xml.WriteEndElement();
                    //ocorrencia 0-1
                    _xml.WriteStartElement("pRedBCST");//Percentual da Redução de BC do ICMS ST
                    _xml.WriteString("1");// tam 5
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vBCST");//Valor da BC do ICMS ST
                    _xml.WriteString(item.vlr_base_subs.ToString("f2"));// tam 15
                    _xml.WriteEndElement();


                    _xml.WriteStartElement("vICMSST");//Valor do ICMS ST
                    _xml.WriteString(item.vlr_icms_subs.ToString("f2"));// tam 15
                    _xml.WriteEndElement();


                    _xml.WriteStartElement("pICMSST");//Alíquota do imposto do ICMS ST


                    //decimal? pmva = 0;
                    //try
                    //{
                    //    if (item.vlr_base_subs > 0)
                    //    {
                    //        pmva = Math.Round(((item.vlr_base_subs / (item.vlr_total + item.vlr_ipi)) - 1) * 100, 2);
                    //    }
                    //    else
                    //    {
                    //        pmva = 0;
                    //    }
                    //}
                    //catch
                    //{
                    //    pmva = 0;
                    //}

                    //if (pmva <= 0)
                    //{
                    //    pmva = 0;
                    //}


                    _xml.WriteString(item.mvast.ToString());// tam 5


                    _xml.WriteEndElement();


                    #endregion

                    //para o caso de ICMS40
                    #region ICMS40
                    //_xml.WriteStartElement("vICMS");//Valor do ICMS
                    //_xml.WriteString("4.30");// tam 5
                    //_xml.WriteEndElement();
                    #endregion

                    //para o caso de ICMSPart
                    #region ICMSPart
                    //_xml.WriteStartElement("pBCOp");//Percentual da BC operação própria
                    //_xml.WriteString("4");// tam 5
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("UFST");//UF para qual é devido o ICMS ST
                    //_xml.WriteString("00");// tam 2
                    //_xml.WriteEndElement();
                    #endregion

                    //para o caso de ICMS101,201,900
                    #region ICMS101_201_900
                    //_xml.WriteStartElement("pCredSN");//Alíquota aplicável de cálculo do crédito (Simples Nacional).
                    //_xml.WriteString("00");// tam 5
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vCredICMSSN");//Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (Simples Nacional)
                    //_xml.WriteString("00");// tam 15
                    //_xml.WriteEndElement();
                    #endregion

                    _xml.WriteEndElement();

                    #endregion

                    _xml.WriteEndElement();
                    #endregion





                    //Grupo do IPI, ocorrencia 0-1
                    #region IPI
                    _xml.WriteStartElement("IPI");

                    //ocorrencia 0-1
                    //_xml.WriteStartElement("clEnq");//Classe de enquadramento do IPI para Cigarros e Bebidas
                    //_xml.WriteString("2");// tam 5
                    //_xml.WriteEndElement();
                    //ocorrencia 0-1
                    //_xml.WriteStartElement("CNPJProd");//CNPJ do produtor da
                    //mercadoria, quando diferente
                    //do emitente. Somente para os
                    //casos de exportação direta ou
                    //indireta.
                    //_xml.WriteString("2");// tam 14
                    //_xml.WriteEndElement();
                    //ocorrencia 0-1
                    //_xml.WriteStartElement("cSelo");//Código do selo de controle IPI
                    //_xml.WriteString("2");// tam 1-60
                    //_xml.WriteEndElement();
                    //ocorrencia 0-1
                    //_xml.WriteStartElement("qSelo");//Quantidade de selo de controle
                    //_xml.WriteString("2");// tam 1-12
                    //_xml.WriteEndElement();

                    _xml.WriteStartElement("cEnq");//Código de Enquadramento Legal do IPI
                    _xml.WriteString("2");// tam 3
                    _xml.WriteEndElement();

                    #region IPITrib
                    _xml.WriteStartElement("IPITrib");//Grupo do CST 00, 49, 50 e 99
                    _xml.WriteStartElement("CST");//Código da situação tributária do IPI
                    _xml.WriteString("2");// tam 2
                    _xml.WriteEndElement();

                    //Informar os campos O10 e O13 caso o cálculo do IPI seja por alíquota ou os campos O11 e O12 caso o cálculo do IPI seja valor por unidade.
                    _xml.WriteStartElement("vBC");//Valor da BC do IPI
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("pIPI");//Alíquota do IPI
                    _xml.WriteString(item.pipi.ToString());// tam 5
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("qUnid");//Quantidade total na unidade
                                                    //padrão para tributação
                                                    //(somente para os produtos
                                                    //tributados por unidade)
                    _xml.WriteString("0");// tam 3
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vUnid");//Valor por Unidade Tributável
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();
                    _xml.WriteStartElement("vIPI");//Valor do IPI
                    _xml.WriteString(item.vlr_ipi.ToString("f2"));// tam 3
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("cEnq");//Código de Enquadramento Legal do IPI
                    _xml.WriteString("2");// tam 3
                    _xml.WriteEndElement();
                    //END IPITRIB
                    _xml.WriteEndElement();
                    #endregion
                    //ou

                    #region IPINT
                    //_xml.WriteStartElement("IPINT");//Grupo do CST 01, 02, 03, 04, 51, 52, 53, 54 e 55

                    //_xml.WriteStartElement("CST");//Código da situação tributária do IPI
                    //_xml.WriteString("2");// tam 2
                    //_xml.WriteEndElement();

                    //_xml.WriteEndElement();
                    #endregion

                    //fim tag IPI;
                    _xml.WriteEndElement();
                    #endregion
                    //omitido P - Imposto de Importação e filhos
                    //este pis


                    #region PIS
                    _xml.WriteStartElement("PIS");//Grupo do PIS
                    #region PISComum
                    int _1 = 0;
                    switch (_1)
                    {
                        case 0:
                            _xml.WriteStartElement("PISAliq");//Grupo de PIS tributado pela alíquota
                            break;
                        case 1:
                            _xml.WriteStartElement("PISQtde");//Grupo de PIS tributado por Qtde
                            break;
                        case 2:
                            _xml.WriteStartElement("PISNT");//Grupo de PIS não tributado
                            break;
                        case 3:
                            _xml.WriteStartElement("PISOutr");//Grupo de PIS não tributado
                            break;
                    }

                    _xml.WriteStartElement("CST");//Código de Situação Tributária do PIS
                    _xml.WriteString("2");// tam 2
                    _xml.WriteEndElement();

                    #endregion

                    #region PISAliq_PISOutr
                    _xml.WriteStartElement("vBC");//Valor da Base de Cálculo do PIS
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("pPIS");//Alíquota do PIS (em percentual)
                    _xml.WriteString("");// tam 5
                    _xml.WriteEndElement();

                    #endregion

                    #region PISQtde_PISOutr

                    _xml.WriteStartElement("qBCProd");//Quantidade Vendida
                    _xml.WriteString("");// tam 16
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vAliqProd");//Alíquota do PIS (em reais)
                    _xml.WriteString("");// tam 5
                    _xml.WriteEndElement();

                    #endregion

                    //usado em PISAliq,PISQtde,PISOutr
                    _xml.WriteStartElement("vPIS");//Valor do PIS
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();
                    _xml.WriteEndElement();//end tipo do pis..
                    _xml.WriteEndElement();//end PIS...;
                    #endregion
                    //ou este
                    #region PISST
                    //_xml.WriteStartElement("PISST");//Grupo de PIS Substituição Tributária

                    //_xml.WriteStartElement("vBC");//Valor da Base de Cálculo do PIS
                    //_xml.WriteString("2");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("pPIS");//Alíquota do PIS (em percentual)
                    //_xml.WriteString("2");// tam 5
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("qBCProd");//Quantidade Vendida
                    //_xml.WriteString("2");// tam 16
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vAliqProd");//Alíquota do PIS (em reais)
                    //_xml.WriteString("2");// tam 5
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vPIS");//Valor do PIS
                    //_xml.WriteString("2");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteEndElement();
                    #endregion

                    //este COFINS
                    #region COFINS
                    _xml.WriteStartElement("COFINS");//Grupo do COFINS
                    #region COFINSComum
                    switch (_1)
                    {
                        case 0:
                            _xml.WriteStartElement("COFINSAliq");//Grupo de COFINS tributado pela alíquota
                            break;
                        case 1:
                            _xml.WriteStartElement("COFINSQtde");//Grupo de COFINS tributado por Qtde
                            break;
                        case 2:
                            _xml.WriteStartElement("COFINSNT");//Grupo de COFINS não tributado
                            break;
                        case 3:
                            _xml.WriteStartElement("COFINSOutr");//Grupo de COFINS não tributado
                            break;
                    }
                    _xml.WriteStartElement("CST");//Código de Situação Tributária do COFINS
                    _xml.WriteString("");// tam 2
                    _xml.WriteEndElement();

                    #endregion

                    #region PISAliq_PISOutr
                    _xml.WriteStartElement("vBC");//Valor da Base de Cálculo do COFINS
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("pCOFINS");//Alíquota da COFINS (em percentual)
                    _xml.WriteString("");// tam 5
                    _xml.WriteEndElement();
                    #endregion

                    #region PISQtde_PISOutr

                    _xml.WriteStartElement("qBCProd");//Quantidade Vendida
                    _xml.WriteString("");// tam 16
                    _xml.WriteEndElement();

                    _xml.WriteStartElement("vAliqProd");//Alíquota do COFINS (em reais)
                    _xml.WriteString("");// tam 5
                    _xml.WriteEndElement();

                    #endregion

                    //usado em COFINSAliq,COFINSQtde,COFINSOutr
                    _xml.WriteStartElement("vCOFINS");//Valor do COFINS
                    _xml.WriteString("");// tam 15
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();//end COFINS...;

                    _xml.WriteEndElement();//end COFINS;
                    #endregion


                    //ou este
                    #region COFINSST
                    //_xml.WriteStartElement("COFINSST");//Grupo de COFINS Substituição Tributária

                    //_xml.WriteStartElement("vBC");//Valor da Base de Cálculo do COFINS
                    //_xml.WriteString("2");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("pCOFINS");//Alíquota do COFINS (em percentual)
                    //_xml.WriteString("2");// tam 5
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("qBCProd");//Quantidade Vendida
                    //_xml.WriteString("2");// tam 16
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vAliqProd");//Alíquota do COFINS (em reais)
                    //_xml.WriteString("2");// tam 5
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vCOFINS");//Valor do COFINS
                    //_xml.WriteString("2");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteEndElement();
                    #endregion
                    //ocorrencia 0-1
                    #region ISSQN
                    //_xml.WriteStartElement("ISSQN");//Grupo do ISSQN

                    //_xml.WriteStartElement("vBC");//Valor da Base de Cálculo do ISSQN
                    //_xml.WriteString("2");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vAliq");//Alíquota do ISSQN
                    //_xml.WriteString("2");// tam 5
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("vISSQN");//Valor do ISSQN
                    //_xml.WriteString("2");// tam 15
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("cMunFG");//Código do município de ocorrência do fato gerador do ISSQN
                    //_xml.WriteString("2");// tam 7
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("cListServ");//Item da Lista de Serviços
                    //_xml.WriteString("2");// tam 3-4
                    //_xml.WriteEndElement();

                    //_xml.WriteStartElement("cSitTrib");//Código de Tributação do ISSQN
                    //_xml.WriteString("2");// tam 1
                    //_xml.WriteEndElement();

                    //_xml.WriteEndElement();
                    #endregion
                    _xml.WriteEndElement();//end element Imposto
                    #endregion

                    _xml.WriteStartElement("infAdProd");//Informações Adicionais do Produto
                    _xml.WriteString("");// tam 500
                    _xml.WriteEndElement();

                    _xml.WriteEndElement();
                    incrementoProd++;
                }
                //fim det
                #endregion

                #region Total
                _xml.WriteStartElement("total");//Grupo de Valores Totais da NF-e

                #region ICMSTot

                _xml.WriteStartElement("ICMSTot");//Grupo de Valores Totais referentes ao ICMS

                _xml.WriteStartElement("vBC");//Valor da BC do ICMS
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vICMS");//Valor do ICMS
                _xml.WriteString("");// tam 5
                _xml.WriteEndElement();

                _xml.WriteStartElement("vBCST");//Valor da BC do ICMS ST
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vST");//Valor da BC do ICMS ST
                _xml.WriteString(itens.Sum(p => (decimal?)p.vlr_icms_subs).ToString());// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vProd");//Valor Total Bruto dos Produtos ou Serviços
                _xml.WriteString(itens.Sum(a => (decimal?)a.vlr_total).ToString());// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vFrete");//Valor Total do Frete
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vSeg");//Valor Total do Seguro
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vDesc");//Valor do Desconto
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vII");//Valor Total do II
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vIPI");//Valor do IPI
                _xml.WriteString(itens.Sum(a => (decimal?)a.vlr_ipi).ToString());// tam 3
                _xml.WriteEndElement();

                _xml.WriteStartElement("vPIS");//Valor do PIS
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vCOFINS");//Valor do COFINS
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vOutro");//Outras despesas acessórias
                                                 //_xml.WriteString(itens.Sum(p => (decimal?)p.vlr_ipi + (decimal?)p.vlr_icms_subs).ToString());// tam 15
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();

                _xml.WriteStartElement("vNF");//Valor Total da NF-e
                _xml.WriteString(itens.Sum(p => (decimal?)p.vlr_total + (decimal?)p.vlr_ipi + (decimal?)p.vlr_icms_subs).ToString());// tam 15
                _xml.WriteEndElement();

                _xml.WriteEndElement();

                #endregion
                ////ocorrencia 0-1
                //#region ISSQNtot

                //_xml.WriteStartElement("ISSQNtot");//Grupo de Valores Totais referentes ao ISSQN
                //                                   //ocorrencia 0-1
                //_xml.WriteStartElement("vServ");//Valor Total dos Serviços sob não-incidência ou não tributados pelo ICMS
                //_xml.WriteString("43195");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vBC");//Valor da BC do ICMS
                //_xml.WriteString("43195");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vISS");//Valor Total do ISS
                //_xml.WriteString("4.30");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vPIS");//Valor do PIS
                //_xml.WriteString("2");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vCOFINS");//Valor do COFINS
                //_xml.WriteString("2");// tam 15
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();

                //#endregion
                ////ocorrencia 0-1
                //#region retTrib

                //_xml.WriteStartElement("retTrib");//Grupo de Retenções de Tributos
                //                                  //ocorrencia 0-1
                //_xml.WriteStartElement("vRetPIS");//Valor Retido de PIS
                //_xml.WriteString("43195");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vRetCOFINS");//Valor Retido de COFINS
                //_xml.WriteString("43195");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vRetCSLL");//Valor Retido de CSLL
                //_xml.WriteString("4.30");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vBCIRRF");//Base de Cálculo do IRRF
                //_xml.WriteString("2");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vIRRF");//Valor Retido do IRRF
                //_xml.WriteString("2");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vBCRetPrev");//Base de Cálculo da Retenção da Previdência Social
                //_xml.WriteString("2");// tam 15
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("vRetPrev");//Valor da Retenção da Previdência Social
                //_xml.WriteString("2");// tam 15
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();

                //#endregion

                _xml.WriteEndElement();
                #endregion

                #region transp

                _xml.WriteStartElement("transp");//Grupo de Informações do Transporte da NF-e

                _xml.WriteStartElement("modFrete");//Modalidade do frete
                _xml.WriteString("0");// tam 15
                _xml.WriteEndElement();

                #region transporta
                //ocorrencia 0-1
                _xml.WriteStartElement("transporta");//Grupo Transportador
                                                     //ocorrencia 0-1
                _xml.WriteStartElement("CNPJ");//Valor Retido de CSLL
                _xml.WriteString("0");// tam 14
                _xml.WriteEndElement();
                //ou
                //_xml.WriteStartElement("CPF");//Valor Retido de CSLL
                //_xml.WriteString("4.30");// tam 11
                //_xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("xNome");//Razão Social ou nome
                _xml.WriteString("DEFINIR");// tam 1-60
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("IE");//Inscrição Estadual
                _xml.WriteString("0");// tam 0,2-14
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("xEnder");//Endereço Completo
                _xml.WriteString("0");// tam 1-60
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("xMun");//Nome do município
                _xml.WriteString("0");// tam 1-60
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("UF");//Sigla da UF
                _xml.WriteString("PR");// tam 15
                _xml.WriteEndElement();
                _xml.WriteEndElement();
                #endregion

                #region retTransp
                //ocorrencia 0-1
                //_xml.WriteStartElement("retTransp");//Grupo de Retenção do ICMS do transporte
                //_xml.WriteStartElement("vServ");//Valor do Serviço
                //_xml.WriteString("0");// tam 15
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("vBCRet");//BC da Retenção do ICMS
                //_xml.WriteString("0");// tam 15
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("pICMSRet");//Alíquota da Retenção
                //_xml.WriteString("0");// tam 5
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("vICMSRet");//Valor do ICMS Retido
                //_xml.WriteString("0");// tam 15
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("CFOP");//CFOP
                //_xml.WriteString("0");// tam 4
                //_xml.WriteEndElement();
                //_xml.WriteStartElement("cMunFG");//Código do município de ocorrência do fato gerador do ICMS do transporte
                //_xml.WriteString("");// tam 7
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();
                #endregion

                #region veicTransp
                //ocorrencia 0-1
                _xml.WriteStartElement("veicTransp");//Grupo Veículo

                _xml.WriteStartElement("placa");//Placa do Veículo
                _xml.WriteString("0");// tam 1 - 8
                _xml.WriteEndElement();

                _xml.WriteStartElement("UF");//
                _xml.WriteString("0");// tam 2
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("RNTC");//Registro Nacional de Transportador de Carga (ANTT)
                _xml.WriteString("0");// tam 5
                _xml.WriteEndElement();

                _xml.WriteEndElement();
                #endregion

                #region reboque
                //ocorrencia 0-5
                //_xml.WriteStartElement("reboque");//Grupo reboque

                //_xml.WriteStartElement("placa");//Placa do Veículo
                //_xml.WriteString("0");// tam 1 - 8
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("UF");//
                //_xml.WriteString("0");// tam 2
                //_xml.WriteEndElement();
                ////ocorrencia 0-1
                //_xml.WriteStartElement("RNTC");//Registro Nacional de Transportador de Carga (ANTT)
                //_xml.WriteString("2");// tam 5
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();
                #endregion
                //OMITIDOS VAGAO E BALSA
                #region vol
                //ocorrencia 0-1
                _xml.WriteStartElement("vol");//Grupo Volumes
                                              //ocorrencia 0-1
                _xml.WriteStartElement("qVol");//Quantidade de volumes transportados
                _xml.WriteString("0");// tam 1-15
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("esp");//Espécie dos volumes transportados
                _xml.WriteString("CAIXAS");// tam 1-60
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("marca");//Marca dos volumes transportados
                _xml.WriteString("");// tam 1-60
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("nVol");//Numeração dos volumes transportados
                _xml.WriteString("");// tam 1-60
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("pesoL");//Peso Líquido (em kg)
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();
                //ocorrencia 0-1
                _xml.WriteStartElement("pesoB");//Peso Bruto (em kg)
                _xml.WriteString("");// tam 15
                _xml.WriteEndElement();
                //ocorrencia 0-N
                //_xml.WriteStartElement("lacres");//Grupo lacres
                //_xml.WriteStartElement("nLacre");//Número dos Lacres
                //_xml.WriteString("2");// tam 1-60
                //_xml.WriteEndElement();
                //_xml.WriteEndElement();

                _xml.WriteEndElement();
                #endregion
                _xml.WriteEndElement();
                #endregion

                #region cobr
                //_xml.WriteStartElement("cobr");//Grupo de Cobrança
                //_xml.WriteStartElement("fat");//Grupo da Fatura
                //_xml.WriteStartElement("nFat");//Número da Fatura
                //_xml.WriteString("43195");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("vOrig");//Valor Original da Fatura
                //_xml.WriteString("43195");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("vDesc");//Valor do desconto
                //_xml.WriteString("43195");// tam 15
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("vLiq");//Valor Líquido da Fatura
                //_xml.WriteString("43195");// tam 15
                //_xml.WriteEndElement();
                //_xml.WriteEndElement();
                ////ocorrencia 0-n (colocar em um for)
                //_xml.WriteStartElement("dup");//Grupo da Duplicata
                //_xml.WriteStartElement("nDup");//Número da Duplicata
                //_xml.WriteString("43195");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("dVenc");//Data de vencimento Formato “AAAA-MM-DD”
                //_xml.WriteString("43195");//
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("vDup");//Valor da duplicata
                //_xml.WriteString("43195");// tam 15
                //_xml.WriteEndElement();
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();
                #endregion

                #region infAdic
                _xml.WriteStartElement("infAdic");//Grupo de Informações Adicionais

                //_xml.WriteStartElement("fat");//Informações Adicionais de Interesse do Fisco
                //_xml.WriteString("43195");// tam 1-2000
                //_xml.WriteEndElement();

                _xml.WriteStartElement("infCpl");//Informações Complementares de interesse do Contribuinte

                _xml.WriteString(Obs);// tam 1-5000

                //_xml.WriteString(string.Format("BASE ICMS: {0} ", itens.Sum(a => (decimal?)a.vlr_total).Value.ToString("C")));// tam 1-5000
                //_xml.WriteString(string.Format("<br />VALOR DO ICMS: {0} ", itens.Sum(a => (decimal?)a.vlr_icms).Value.ToString("C")));// tam 1-5000
                //_xml.WriteString(string.Format("<br />VALOR DO IPI: {0} ", itens.Sum(a => (decimal?)a.vlr_ipi).Value.ToString("C")));// tam 1-5000
                //_xml.WriteString(string.Format("<br />BASE ST: {0} ", itens.Sum(a => (decimal?)a.vlr_base_subs).Value.ToString("C")));// tam 1-5000
                //_xml.WriteString(string.Format("<br />VALOR DO ICMS-ST: {0} ", itens.Sum(a => (decimal?)a.vlr_icms_subs).Value.ToString("C")));// tam 1-5000


                _xml.WriteEndElement();



                #region obsCont
                //_xml.WriteStartElement("obsCont");//GGrupo do campo de uso livre do contribuinte

                //_xml.WriteStartElement("xCampo");//Identificação do campo
                //_xml.WriteString(" EMPRESA DO SIMPLES ");// tam 1-20
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("xTexto");//Conteúdo do campo
                //_xml.WriteString(" EMPRESA DO SIMPLES ");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();
                #endregion

                #region obsFisco
                //_xml.WriteStartElement("obsFisco");//Grupo do campo de uso livre do Fisco

                //_xml.WriteStartElement("xCampo");//Identificação do campo
                //_xml.WriteString(" EMAIL: foxlux@foxlux.com.br");// tam 1-20
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("xTexto");//Conteúdo do campo
                //_xml.WriteString(" EMPRESA DO SIMPLES ");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();
                #endregion

                #region procRef
                //_xml.WriteStartElement("procRef");//Grupo do processo referenciado

                //_xml.WriteStartElement("nProc");//Indentificador do processo ou ato concessório
                //_xml.WriteString("43195");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("indProc");//Indicador da origem do processo
                //_xml.WriteString("43195");// tam 1
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();
                #endregion
                //_xml.WriteEndElement();
                #endregion

                //omitidos ZA - Informações de Comércio Exterior e filhos
                #region compra
                // se tratar de compra publica
                //_xml.WriteStartElement("compra");//GGrupo do campo de uso livre do contribuinte

                //_xml.WriteStartElement("xNEmp");//Nota de Empenho
                //_xml.WriteString("43195");// tam 1-17
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("xPed");//Pedido
                //_xml.WriteString("43195");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteStartElement("xCont");//Contrato
                //_xml.WriteString("43195");// tam 1-60
                //_xml.WriteEndElement();

                //_xml.WriteEndElement();
                #endregion
                //Omitidos ZC - Informações do Registro de Aquisição de Cana

                //Omitido ZZ - Informações da Assinatura Digital

                //END InfNFE
                _xml.WriteEndElement();//InfNFE
                #endregion
                //end nfe
                _xml.WriteEndElement();//Nfe
                #endregion
                //end envnfe
                _xml.WriteEndElement();//envNFE
                #endregion
                _xml.Close();
                //MessageBox.Show("Arquivo exportado");




            }
            catch (Exception ex)
            {
                Response.Write("Erro ao Gerar XML" + ex.ToString());

            }

            return caminho;
        }




        public string GeraXMLoLD(int id)
        {
            string cod_pessoa = db.Garantia.Where(a => a.garantiaid == id).Select(p => p.cod_cliente).First().ToString();

            var DadosDoCliente = (
                            from a in db.Dados_crm.Where(a => a.cod_pessoa == cod_pessoa)
                            select new ClienteXML
                            {
                                CEP = a.cep,
                                Cpf = a.cgc_cpf,
                                nro = "0",
                                UF = a.des_estado,
                                xBairro = a.bairro,
                                xCpl = "",
                                xLgr = a.des_endereco,
                                xMun = a.des_cidade,
                                xNome = a.des_fantasia
                            }
                         ).ToList();

            List<XElement> elements = new List<XElement>();
            elements.Add(new XElement("dest",
                                    new XElement("CPF", DadosDoCliente.Single().Cpf),
                                    new XElement("xNome", DadosDoCliente.Single().xNome),
                                    new XElement("enderDest",
                                        new XElement("xLgr", DadosDoCliente.Single().xLgr),
                                        new XElement("nro", DadosDoCliente.Single().nro),
                                        new XElement("xCpl", DadosDoCliente.Single().xCpl),
                                        new XElement("xBairro", DadosDoCliente.Single().xBairro),
                                        new XElement("cMun", 0),
                                        new XElement("xMun", DadosDoCliente.Single().xMun),
                                        new XElement("UF", DadosDoCliente.Single().UF),
                                        new XElement("CEP", DadosDoCliente.Single().CEP),
                                        new XElement("cPais", "1058"),
                                        new XElement("xPais", "BRASIL")
                                        )));

            XDocument xsefaz = new XDocument(
                                       new XElement("sistema", elements)
                            );


            string xml = xsefaz.ToString();

            string caminho = Path.Combine(HttpContext.Server.MapPath(string.Format("~/Content/XmlDeNotas/{0}.xml", id)));

            //Salvando XML
            xsefaz.Save(caminho);

            return caminho;



        }



        public FileStreamResult ImpressaoNFD_old(int id, string obs, bool agrupa)
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            string imagepath = Server.MapPath("~/Content");
            Image gif = Image.GetInstance(imagepath + "/Logo_P.gif");
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial8 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font fverdana = FontFactory.GetFont("Verdana", 12, Color.BLACK);

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.ITALIC,
              Color.GREEN
              );
            Font smallfont = FontFactory.GetFont("Arial", 7);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");


            document.Open();
            document.Add(gif);
            document.Add(new Paragraph(" "));

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();

            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("NOTA DO CLIENTE", verdana));
            Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, Color.LIGHT_GRAY, Element.ALIGN_CENTER, -1));

            document.Add(_TITULO);
            //document.Add(new Paragraph("______________________________________________________________________________"));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Dados do Cliente:", arial8));


            PdfContentByte cb = writer.DirectContent;
            document.Add(linebreak);

            PdfPTable table_cliente = new PdfPTable(2);
            table_cliente.TotalWidth = 520f;
            table_cliente.LockedWidth = true;
            table_cliente.DefaultCell.BorderWidth = 1;
            float[] widths = new float[] { 80f, 200f };
            table_cliente.SetWidths(widths);
            table_cliente.HorizontalAlignment = 0;
            //table_cliente.SpacingBefore = 20f;
            //table_cliente.SpacingAfter = 30f;


            fverdana.Size = 8;
            Paragraph paragrafo = new Paragraph();

            table_cliente.AddCell(new Chunk("Nº  : ", fverdana).ToString());
            table_cliente.AddCell(new Chunk(id.ToString(), fverdana).ToString());

            document.Add(table_cliente);

            //document.Add(paragrafo);

            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Cód. Garantia: {0}", id.ToString()), fverdana));
            document.Add(paragrafo);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");
        }

        public FileStreamResult ImpressaoColeta(int id, bool todas = false)
        {
            int _cod_representante = id;

            if (todas)
            {
                id = db.Garantia.Where(a => a.cod_representante == _cod_representante &&
                a.ind_emitido_nf == "S" && a.ind_cancelada == "N" && a.ind_emitido_coleta == "S" && a.dta_finalizacao == null
                ).Select(a => a.garantiaid).Min();
            }


            Garantia DadosGarantia = db.Garantia.Find(id);
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(iTextSharp.text.PageSize.A5);
            Document document = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;

            if (DadosGarantia == null)
            {

                writer.PageEvent = new PDFFooter(titulo: "ORDEM DE COLETA", canhoto: true);
                document.Open();
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                document.Close();

                byte[] byteInfoS = workStream.ToArray();
                workStream.Write(byteInfoS, 0, byteInfoS.Length);
                workStream.Position = 0;


                return new FileStreamResult(workStream, "application/pdf");
            }


            DadosDoCrm DadosCliente = db.Dados_crm.Find(DadosGarantia.cod_cliente.ToString());
            Ps_Representantes DadosRepresentante = db.Ps_Representantes.Find(DadosGarantia.cod_representante);
            TRANSPORTADOR transp = db.TRANSPORTADOR.Where(a => a.COD_TRANSPORTADOR == DadosGarantia.cod_transportador).FirstOrDefault();
            var Itens = db.GarantiaItem.Where(a => a.garantiaid == id).ToList();


            var Garantias = db.Garantia.Where(a => a.cod_representante == DadosRepresentante.cod_pessoa &&
            a.ind_emitido_nf == "S" && a.ind_cancelada == "N" && a.ind_emitido_coleta == "S" && a.dta_finalizacao == null).ToList();


            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial8 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font fverdana = FontFactory.GetFont("Verdana", 12, Color.BLACK);

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.NORMAL, Color.ORANGE
              );
            Font smallfont = FontFactory.GetFont("Arial", 6, Color.GRAY);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");

            Font palatino_i = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.NORMAL, Color.BLACK
              );


            writer.PageEvent = new PDFFooter(titulo: "ORDEM DE COLETA", canhoto: true);

            document.Open();

            //gif.ScalePercent(45);
            //document.Add(gif);
            document.Add(new Paragraph(" "));

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();

            float[] largura_colunas = new float[] { 1f, 2f };
            Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, Color.LIGHT_GRAY, Element.ALIGN_CENTER, -1));
            PdfPCell celula = new PdfPCell();







            Paragraph _TituloParagrafo = new Paragraph();
            _TituloParagrafo.Alignment = Element.ALIGN_LEFT;






            PdfPTable header = new PdfPTable(2);
            header.TotalWidth = document.PageSize.Width - 10;
            header.LockedWidth = true;
            header.DefaultCell.BorderWidth = 1;
            header.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] largura_col = new float[] { 30f, 100f };
            header.SetWidths(largura_col);




            //linha do numero
            celula = new PdfPCell(new Paragraph(new Chunk("Número:", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(DadosGarantia.garantiaid.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            //linha dataprevista
            celula = new PdfPCell(new Paragraph(new Chunk("Previsão:", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            if (!todas)
            {
                celula = new PdfPCell(new Paragraph(new Chunk(DadosGarantia.dta_previsao_coleta.Value.ToShortDateString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            }
            else
            {
                celula = new PdfPCell(new Paragraph(new Chunk(Garantias.Min(a => a.dta_previsao_coleta.Value.ToShortDateString()), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            }

            header.AddCell(celula);




            //transportador 
            celula = new PdfPCell(new Paragraph(new Chunk("Transportadora:", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(transp != null ? transp.FANTASIA : "", palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            //obs
            celula = new PdfPCell(new Paragraph(new Chunk("Obs:", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(DadosGarantia.obscoleta.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            document.Add(header);


            document.Add(new Paragraph(" "));

            PdfPTable local = new PdfPTable(4);
            local.TotalWidth = document.PageSize.Width - 10;
            local.LockedWidth = true;
            local.DefaultCell.BorderWidth = 0;
            local.DefaultCell.BorderColor = Color.BLACK;
            local.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] largura_coluna_local = new float[] { 30f, 50f, 30f, 100f };
            local.SetWidths(largura_coluna_local);
            //local.SpacingBefore = 20f;
            //local.SpacingAfter = 30f;


            celula = new PdfPCell(new Paragraph(new Chunk("Solicitante", palatino_i))) { Border = 0, Colspan = 2 };
            local.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Local Coleta", palatino_i))) { Border = 0, Colspan = 2 };
            local.AddCell(celula);


            string Nome = todas ? DadosRepresentante.des_pessoa : DadosCliente.des_pessoa;
            string Endereco = todas ? DadosRepresentante.des_endereco : DadosCliente.des_endereco;



            celula = new PdfPCell(new Paragraph(new Chunk("Nome", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Foxlux", palatino_i))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Nome", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(Nome, palatino_i))) { Border = 0 };
            local.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("End.", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Santa Helena, 894", palatino_i))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("End.", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(Endereco, palatino_i))) { Border = 0 };
            local.AddCell(celula);

            string Cidade = todas ? DadosRepresentante.des_cidade + "-" + DadosRepresentante.uf : DadosCliente.des_cidade + "-" + DadosCliente.uf;


            celula = new PdfPCell(new Paragraph(new Chunk("Cidade", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Pinhais-PR", palatino_i))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Cidade", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(Cidade, palatino_i))) { Border = 0 };
            local.AddCell(celula);


            string Bairro = todas ? DadosRepresentante.bairro : DadosCliente.bairro;

            celula = new PdfPCell(new Paragraph(new Chunk("Bairro", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Emiliano Perneta", palatino_i))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Bairro", palatino))) { Border = 0 };
            local.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(Bairro, palatino_i))) { Border = 0 };
            local.AddCell(celula);


            //Nome

            document.Add(local);

            document.Add(linebreak);
            document.Add(new Chunk(" "));




            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = document.PageSize.Width - 10;
            table.LockedWidth = true;
            table.DefaultCell.BorderWidth = 0;
            table.HorizontalAlignment = 1;
            float[] largura_colunas_coleta = new float[] { 100f, 100f, 240f, 200f };
            table.SetWidths(largura_colunas_coleta);


            celula = new PdfPCell(new Paragraph(new Chunk("Dados da Carga a Ser Coletada", palatino))) { Colspan = 4, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Coleta", palatino)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Volumes", palatino)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Descrição", palatino)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Nota Fiscal", palatino)));
            table.AddCell(celula);

            if (!todas)
            {

                celula = new PdfPCell(new Paragraph(new Chunk(DadosGarantia.num_coleta.ToString(), palatino_i)));
                table.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(DadosGarantia.volumes, palatino_i)));
                table.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(DadosGarantia.especies, palatino_i)));
                table.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(DadosGarantia.num_nf_cliente, palatino_i)));
                table.AddCell(celula);
            }
            else
            {
                foreach (var gat in Garantias)
                {
                    celula = new PdfPCell(new Paragraph(new Chunk(gat.num_coleta.ToString(), palatino_i)));
                    table.AddCell(celula);

                    celula = new PdfPCell(new Paragraph(new Chunk(gat.volumes, palatino_i)));
                    table.AddCell(celula);

                    celula = new PdfPCell(new Paragraph(new Chunk(gat.especies, palatino_i)));
                    table.AddCell(celula);

                    celula = new PdfPCell(new Paragraph(new Chunk(gat.num_nf_cliente, palatino_i)));
                    table.AddCell(celula);

                }

            }



            document.Add(table);




            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");
        }




        [HttpPost]
        [ValidateInput(false)]
        public ActionResult VinculaNF(int id, string NF_CLiente, string Obs)
        {

            Garantia data_update = db.Garantia.Find(id);
            data_update.ind_emitido_nf = "S";
            data_update.cod_status = 6;
            data_update.num_nf_cliente = NF_CLiente;

            Garantia_Troca_Status altera_linha = AddDataSaidaEstagioAnterior(data_update.garantiaid);
            Garantia_Troca_Status nova_linha = AddNovaLinhaTrocaEstagio(data_update.garantiaid, data_update.cod_status, 6, Obs);

            db.Entry(data_update).State = EntityState.Modified;
            db.Entry(altera_linha).State = EntityState.Modified;
            db.Garantia_Troca_Status.Add(nova_linha);


            for (int i = 0; i <= Request.Files.Count - 1; i++)
            {
                int tamanho = (int)Request.Files[i].InputStream.Length;
                string contentype = Request.Files[i].ContentType;
                byte[] arq = new byte[tamanho];

                Request.Files[i].InputStream.Read(arq, 0, tamanho);
                byte[] arqUp = arq;



                int nextt = db.Database.SqlQuery<Int32>("select GarantiaArqSeq.NextVal from dual ").FirstOrDefault<Int32>();
                string NomeArquivo = id.ToString() + '_' + nextt.ToString() + '_' + string.Format("{0}", Path.GetFileName(Request.Files[i].FileName));

                GarantiaArq cat = new GarantiaArq
                {
                    id = nextt,
                    garantiaid = id,
                    caminho = NomeArquivo,
                    des_contenttype = contentype,
                    des_imagem = arqUp,
                    nome_arquivo = Request.Files[i].FileName
                };


                db.GarantiaArq.Add(cat);
                var path = Path.Combine(Server.MapPath("~/Arquivos/uploads"), NomeArquivo);
                Request.Files[i].SaveAs(path);
            }


            try
            {
                db.SaveChanges();

            }
            catch
            {
                return InvokeHttpNotFound();
            }


            return RedirectToAction("List");

        }

        [HttpPost]
        public ActionResult GeraOrdemColeta(int id,
            int cod_transportador,  //ok
            string ind_coleta_direta, //ok
            string num_coleta, //ok
            DateTime dta_previsao, //ok
            string Obs, // ok
            string volumes,
            string especie
            )
        {
            Garantia data_update = db.Garantia.Find(id);
            data_update.ind_emitido_coleta = "S";
            data_update.ind_coleta_direta = ind_coleta_direta;
            data_update.num_coleta = string.IsNullOrEmpty(num_coleta) ? db.Database.SqlQuery<Int32>("select OrdemColeta_Seq.NextVal from dual ").FirstOrDefault<Int32>().ToString() : num_coleta;
            data_update.cod_status = 5;
            data_update.cod_transportador = cod_transportador;
            data_update.dta_previsao_coleta = dta_previsao;
            data_update.dta_coleta = System.DateTime.Now;
            data_update.obscoleta = Obs;
            data_update.volumes = volumes;
            data_update.especies = especie;


            Garantia_Troca_Status altera_linha = AddDataSaidaEstagioAnterior(data_update.garantiaid);
            Garantia_Troca_Status nova_linha = AddNovaLinhaTrocaEstagio(data_update.garantiaid, data_update.cod_status, 6, Obs);

            db.Entry(data_update).State = EntityState.Modified;
            db.Entry(altera_linha).State = EntityState.Modified;
            db.Garantia_Troca_Status.Add(nova_linha);

            try
            {
                db.SaveChanges();
                return Json(new { id = data_update.garantiaid, Msg = "Ok, Gerado OC com sucesso ", hasError = false });
            }
            catch (Exception e)
            {
                return Json(new { id = data_update.garantiaid, Msg = e.Message, hasError = true });
            }

        }


        public PartialViewResult GetDetailsFromDoc(int id)
        {
            var Itens = db.Garantia_Troca_Status.Where(a => a.garantiaId == id).OrderByDescending(a => a.num_seq).ToList();
            return PartialView(Itens);
        }


        public ActionResult ReadItemJsUsingSql(ParametersDataTable param, int id)
        {

            //int total_de_linhas = db.IE_Itens_Vendas.AsNoTracking().Where(a => a.cod_cliente == id).Count();
            var allItem = db.Database.SqlQuery<V_Garantias_Representantes>(string.Format(SQL_GET_GARANTIA_POR_REPRES, id));
            IEnumerable<V_Garantias_Representantes> filteredCompanies;


            if ((!string.IsNullOrEmpty(param.sSearch)))
            {
                filteredCompanies =
                    allItem.Where(c => c.razao.ToUpper().Contains(param.sSearch.ToUpper())
                                      || (c.num_coleta != null && c.num_coleta.ToUpper().Contains(param.sSearch.ToUpper()))
                                      || (c.num_nf_cliente != null && c.num_nf_cliente.ToUpper().Contains(param.sSearch.ToUpper()))
                                      || (c.obs != null && c.obs.ToUpper().Contains(param.sSearch.ToUpper()))
                                      || (c.obscoleta != null && c.obscoleta.ToUpper().Contains(param.sSearch.ToUpper()))
                );
            }
            else
            {
                filteredCompanies = allItem;
            }

            filteredCompanies = filteredCompanies.OrderByDescending(c => c.dta_previsao_coleta);

            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);

            var Vlr_Total = allItem.Select(p => p.vlr_garantia).DefaultIfEmpty(0).Sum();

            var result = from c in displayedCompanies
                         select new
                         {
                             cod_cliente = c.cod_cliente,
                             cod_representante = c.cod_representante,
                             des_nome = c.des_nome,
                             dta_inclusao = c.dta_inclusao.ToShortDateString(),
                             dta_previsao_coleta = c.dta_previsao_coleta.ToShortDateString(),
                             dta_coleta = c.dta_coleta.ToShortDateString(),
                             especies = c.especies,
                             garantiaid = c.garantiaid,
                             num_coleta = c.num_coleta,
                             num_nf_cliente = c.num_nf_cliente,
                             obs = c.obs,
                             obscoleta = c.obscoleta,
                             razao = c.razao,
                             vlr_garantia = c.vlr_garantia,
                             volumes = c.volumes,
                             Percentual = (IntervaloDias(c.dta_coleta, c.dta_previsao_coleta, System.DateTime.Now)),
                             RepresentacaoTotal = Decimal.Round((c.vlr_garantia / Vlr_Total) * 100, 0),
                             Procedimento = c.cod_procedimento_vinculado,
                             Check = 0.ToString(),
                         };

            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allItem.Count(),
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;
        }


        private double IntervaloDias(DateTime dataInicio, DateTime dataFim, DateTime dataIntermediaria)
        {
            long intervaloTotal = Math.Abs(dataFim.Ticks - dataInicio.Ticks);
            long intervaloInter = Math.Abs(dataIntermediaria.Ticks - dataInicio.Ticks);
            return intervaloInter * 100 / intervaloTotal;
        }

        public ActionResult ShowOrdersReseller(int id)
        {
            var garantiasTemporario = db.Tmp_Garantia_Impressao.Where(a => a.cod_usuario == cd_usuario).ToList();

            garantiasTemporario.ForEach(tmp => db.Tmp_Garantia_Impressao.Remove(tmp));
            db.SaveChanges();

            var data = db.Database.SqlQuery<V_Garantias_Representantes>(string.Format(SQL_GET_GARANTIA_POR_REPRES, id)).ToList().Take(1);
            return View(data);
        }






        public PartialViewResult GetDocs(int id)
        {

            var Itens = db.GarantiaArq.Where(a => a.garantiaid == id).ToList();
            ViewBag.GarantiaId = id;

            return PartialView(Itens);
        }

        [HttpPost]
        public ActionResult RemoveDocFromGarantia(int id)
        {
            // Remove the item from the cart
            GarantiaArq varDelete = db.GarantiaArq.Where(p => p.id == id).Single();
            db.GarantiaArq.Remove(varDelete);

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
        public ActionResult AddDocGarantia(int garantiaid)
        {
            for (int i = 0; i <= Request.Files.Count - 1; i++)
            {
                int tamanho = (int)Request.Files[i].InputStream.Length;
                string contentype = Request.Files[i].ContentType;
                byte[] arq = new byte[tamanho];

                Request.Files[i].InputStream.Read(arq, 0, tamanho);
                byte[] arqUp = arq;


                int nextt = db.Database.SqlQuery<Int32>("select GarantiaArqSeq.NextVal from dual ").FirstOrDefault<Int32>();
                string NomeArquivo = garantiaid.ToString() + '_' + nextt.ToString() + '_' + string.Format("{0}", Path.GetFileName(Request.Files[i].FileName));

                GarantiaArq cat = new GarantiaArq
                {
                    id = nextt,
                    garantiaid = garantiaid,
                    caminho = NomeArquivo,
                    des_contenttype = contentype,
                    des_imagem = arqUp,
                    nome_arquivo = Request.Files[i].FileName
                };

                db.GarantiaArq.Add(cat);
            }


            try
            {
                db.SaveChanges();

            }
            catch
            {
                return InvokeHttpNotFound();
            }

            return RedirectToAction("GetDocs", new { id = garantiaid });
        }




        public void HtmlToPdf(int id)
        {
            SendEmail email = new SendEmail();
            string body = db.GarantiaArq.Where(a => a.id == id).Select(a => a.des_imagem).ToString();


            email.EnviarEmailSacCliente(881, "", body);

        }


        public ActionResult PartialView(int id)
        {
            var docs = (from a in db.GarantiaArq
                        where a.id == id
                        select new
                        {
                            doc = a.des_imagem,
                            tipo = a.des_contenttype
                        }).ToList();

            byte[] imagedata = (byte[])docs[0].doc;

            return File(imagedata, docs[0].tipo);


            //if (imagedata == null)
            //{
            //    return File("blank.gif", "image/png");
            //}
            //else
            //return File(imagedata, "image/png");


        }

        public ActionResult Download(int id)
        {
            var docs = (from a in db.GarantiaArq
                        where a.id == id
                        select new
                        {
                            doc = a.des_imagem,
                            tipo = a.des_contenttype
                        }).ToList();

            byte[] imagedata = (byte[])docs[0].doc;

            return File(imagedata, docs[0].tipo);


            //if (imagedata == null)
            //{
            //    return File("blank.gif", "image/png");
            //}
            //else
            //return File(imagedata, "image/png");


        }





    }
}
