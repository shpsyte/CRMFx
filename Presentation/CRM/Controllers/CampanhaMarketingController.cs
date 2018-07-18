using CRM.App_Helpers;
using CRM.Extends;
using Data.Context;
using Domain.Entity;
using Services.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Objects.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CRM.Controllers
{
    public class CampanhaMarketingController : BasePublicController
    {
        string sql_base = " SELECT nvl(SUM(Cedia.Vlr_Total),0) " +
                           "  FROM Ce_Diarios CeDia INNER JOIN NS_NOTAS Nota ON Nota.Num_Seq = CeDia.Num_Seq_Ns " +
                           " WHERE Nota.Dta_Emissao BETWEEN \'{0}\'  AND \'{1}\' " +
                           "   AND Nota.cod_cliente = {2} ";


        public ActionResult List()
        {
            //var data = db.CampanhaMarketing.ToList();
            return View();
        }
        public PartialViewResult GetDetailsFromDoc(int id)
        {
            var Itens = db.CampanhaMarketingDoc.Where(a => a.campanhaID == id).OrderByDescending(a => a.CampanhaMarketingDocId).ToList();
            ViewBag.CampanhaId = id;
            return PartialView(Itens);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDocCampanha(int campanhaid)
        {
            for (int i = 0; i <= Request.Files.Count - 1; i++)
            {
                int tamanho = (int)Request.Files[i].InputStream.Length;
                string contentype = Request.Files[i].ContentType;
                byte[] arq = new byte[tamanho];

                Request.Files[i].InputStream.Read(arq, 0, tamanho);
                byte[] arqUp = arq;


                int CampanhaDocId = db.Database.SqlQuery<Int32>("select CampanhaMarketingDoc_Seq.NextVal from dual ").FirstOrDefault<Int32>();
                string NomeArquivo = Request.Files[i].FileName; //data.campanhaID.ToString() + '_' + CampanhaDocId.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));

                CampanhaMarketingDoc doc = new CampanhaMarketingDoc
                {
                    CampanhaMarketingDocId = CampanhaDocId,
                    campanhaID = campanhaid,
                    Caminho = NomeArquivo,
                    des_contenttype = contentype,
                    des_imagem = arqUp,
                    nome_arquivo = NomeArquivo
                };
                db.CampanhaMarketingDoc.Add(doc);
            }


            try
            {
                db.SaveChanges();

            }
            catch
            {
                return InvokeHttpNotFound();
            }

            return RedirectToAction("GetDetailsFromDoc", new { id = campanhaid });
        }




        public ActionResult Download(int id)
        {
            var docs = (from a in db.CampanhaMarketingDoc
                        where a.CampanhaMarketingDocId == id
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
        public ActionResult ReadOrdersUsingSql(ParametersDataTable param)
        {
            //int total_de_linhas = db.IE_Itens_Vendas.AsNoTracking().Where(a => a.cod_cliente == id).Count();
            var allItem = db.CampanhaMarketing.AsNoTracking().Where(a => a.statusId != 4 && a.statusId != 5).ToList();
            IEnumerable<CampanhaMarketing> filteredCompanies;
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<CampanhaMarketing, string> orderingFunction = (c =>
                                                           sortColumnIndex == 0 ? c.campanhaID.ToString() :
                                                           sortColumnIndex == 1 ? c.des_nome :
                                                           sortColumnIndex == 2 ? c.Ps_Pessoas.des_pessoa :
                                                           sortColumnIndex == 4 ? c.Segmentos.des_segmento.ToString() :
                                                           sortColumnIndex == 4 ? c.TipoAcao.des_acao.ToString() :
                                                            c.campanhaID.ToString()
                                                         );
            if ((!string.IsNullOrEmpty(param.sSearch)))
            {
                filteredCompanies = db.CampanhaMarketing.AsNoTracking()
                   .Where(c => c.des_nome.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.Ps_Pessoas.des_pessoa.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.Segmentos.des_segmento.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.TipoAcao.des_acao.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.cod_pessoa_string.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.Ps_Representantes.des_pessoa.ToUpper().Contains(param.sSearch.ToUpper())

                      );
            }
            else
            {
                filteredCompanies = allItem;
            }
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);
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
                             cod_regional = c.cod_regional
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
        public ActionResult WorkFlow()
        {
            return View();
        }
        public ActionResult ReadWorkFlow(ParametersDataTable param)
        {
            var estagios = db.EstagioUsuario.Where(a => a.cd_usuario == cd_usuario).Select(a => a.estagioId).ToList();
            if (estagios.Count() == 0)
            {
                estagios = db.Estagio.Select(a => a.estagioId).ToList();
            }
            var allItem = db.CampanhaMarketing.AsNoTracking().Where(a => estagios.Contains(a.estagioId) 
            && (a.statusId == 1 || a.statusId == 2));
            IEnumerable<CampanhaMarketing> filteredCompanies;
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            //correcao da orddencao pele campo regional
            Func<CampanhaMarketing, string> orderingFunction = (c =>
                                                           sortColumnIndex == 0 ? c.campanhaID.ToString() :
                                                           sortColumnIndex == 1 ? c.campanhaID.ToString() :
                                                           sortColumnIndex == 2 ? c.cod_regional.ToString() :
                                                           sortColumnIndex == 3 ? c.des_nome :
                                                           sortColumnIndex == 4 ? c.Ps_Pessoas.des_pessoa :
                                                           sortColumnIndex == 5 ? c.Segmentos.des_segmento.ToString() :
                                                           sortColumnIndex == 6 ? c.Estagio.descricao.ToString() :
                                                           sortColumnIndex == 7 ? c.Status.descricao.ToString() :
                                                            c.campanhaID.ToString()
                                                         );
            if ((!string.IsNullOrEmpty(param.sSearch)))
            {
                filteredCompanies = allItem.Where(
                            c => c.des_nome.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.Ps_Pessoas.des_pessoa.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.Segmentos.des_segmento.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.TipoAcao.des_acao.ToUpper().Contains(param.sSearch.ToUpper())
                      );
            }
            else
            {
                filteredCompanies = allItem;
            }
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);
            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new
                         {
                             campanhaid = c.campanhaID,
                             cod_regional = c.cod_regional,
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
                             acao = ""
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
        public ActionResult AprovaReprova(int id)
        {
            CampanhaMarketing_Model data = new CampanhaMarketing_Model { Obs = "", CampanhaMarketing = db.CampanhaMarketing.Find(id) };
            bool _ind_finalizada = false;
            if (_ind_finalizada)
            { return RedirectToAction("Details", new { id = id }); }
            string ids = Convert.ToString(id);
            ViewBag.comentarios = db.ListaComentarios.Where(a => a.cod_interno == ids && a.tipo_nota == "CAMPANHA").ToList();
            ViewBag.troca_estagio = db.CampanhaMarketingEstagios.Where(a => a.campanhaId == id).OrderBy(p => p.num_seq).ToList();
            if (data.CampanhaMarketing == null)
                return InvokeHttpNotFound();
            return View(data);
        }
        [HttpPost]
        [ParameterBasedOnFormName("save-continue", "aprovado")]
        public ActionResult AprovaReprova(CampanhaMarketing_Model data, bool aprovado)
        {
            bool _ind_finalizada = false;
            if (_ind_finalizada) { return RedirectToAction("Details", new { id = data.CampanhaMarketing.campanhaID }); }
            CampanhaMarketing data_update = db.CampanhaMarketing.Find(data.CampanhaMarketing.campanhaID);
            if (data_update == null) { return View(data); }
            string ids = data.CampanhaMarketing.campanhaID.ToString();
            ViewBag.comentarios = db.ListaComentarios.Where(a => a.cod_interno == ids && a.tipo_nota == "CAMPANHA").ToList();
            ViewBag.troca_estagio = db.CampanhaMarketingEstagios.Where(a => a.campanhaId == data.CampanhaMarketing.campanhaID).OrderBy(p => p.num_seq).ToList();
            if (ModelState.IsValid)
            {
                int estagioId = 0;
                int statusId = 0;
                Estagio estagio = db.Estagio.Where(a => a.estagioId == data_update.estagioId).FirstOrDefault();


                if (aprovado)
                {
                    estagioId = Convert.ToInt32(estagio.estagioid_proximo);
                    Estagio proximo_estagio = db.Estagio.Where(a => a.estagioId == estagioId).FirstOrDefault();
                    statusId = Convert.ToInt32(proximo_estagio.statusid);
                }
                else
                {
                    estagioId = Convert.ToInt32(estagio.estagioid_anterior);

                    if (data_update.estagioId == 23)
                    {
                        estagioId = 21;
                    }

                    statusId = 2;

                    Estagio estagio_anterior = db.Estagio.Where(a => a.estagioId == estagioId).FirstOrDefault();

                    if (estagio_anterior.ind_liberado == "S")
                        statusId = Convert.ToInt32(estagio_anterior.statusid);

                }
                CampanhaMarketingEstagios altera_linha = AddDataSaidaEstagioAnterior(data_update.campanhaID);
                CampanhaMarketingEstagios nova_linha = AddNovaLinhaTrocaEstagio(data_update.campanhaID, data_update.estagioId, estagioId, data.Obs);
                data_update.dta_alteracao = System.DateTime.Now;
                data_update.cod_usuario_alteracao = cd_usuario;
                data_update.estagioId = estagioId;
                data_update.statusId = statusId;
                data_update.des_ult_obs = data.Obs;
                try
                {
                    db.Entry(data_update).State = EntityState.Modified;
                    db.Entry(altera_linha).State = EntityState.Modified;
                    db.CampanhaMarketingEstagios.Add(nova_linha);
                    db.SaveChanges();
                    _email.EnviarEmailCampanha(data_update.campanhaID, "NovaCampanha.htm");
                    return RedirectToAction("WorkFlow");
                }
                catch (Exception e)
                {
                    Response.StatusCode = 500; // Replace .AddHeader
                    return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return View(data);
        }
        private CampanhaMarketingEstagios AddNovaLinhaTrocaEstagio(int campanhaId, int? cod_estagio_anterior, int? cod_estagio_novo, string msg)
        {
            return new CampanhaMarketingEstagios
            {
                estagioidentrada = cod_estagio_novo,
                estagioidanterior = cod_estagio_anterior,
                cod_usuario = cd_usuario,
                campanhaId = campanhaId,
                num_seq = db.Database.SqlQuery<Int32>("select CampanhaMarketingEstagiosSeq.NextVal from dual ").FirstOrDefault<Int32>(),
                dta_entrada = System.DateTime.Now,
                dta_saida = null,
                obs = msg
            };
        }
        private CampanhaMarketingEstagios AddDataSaidaEstagioAnterior(int campanhaId)
        {
            int? num_seq = db.CampanhaMarketingEstagios.Where(a => a.campanhaId == campanhaId && a.dta_saida == null).Max(p => p.num_seq);
            CampanhaMarketingEstagios data = db.CampanhaMarketingEstagios.Find(num_seq, campanhaId);
            data.dta_saida = System.DateTime.Now;
            return data;
        }

      
        public ActionResult Details(int id)
        {
            ViewBag.tipoacao = new SelectList(db.TipoAcao, "tipoacaoid", "des_acao", string.Empty);
            ViewBag.segmentos = new SelectList(db.Segmentos, "segmentoid", "des_segmento", string.Empty);
            ViewBag.estagio = new SelectList(db.Estagio.Where(a => a.ind_liberado == "N"), "estagioId", "descricao");
            ViewBag.formapgto = new SelectList(db.FormaPgto, "formapgtoid", "des_forma");


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
            {
                try
                {
                    per_atingido = Math.Round(vlr_faturado / Campanha.vlr_meta.Value, 4);

                }catch(Exception e)
                {
                    per_atingido = 0;
                }


                }

            CampanhaMarketingReviewModel data = new CampanhaMarketingReviewModel
            {
                CampanhaMarketing = Campanha,
                ListaComentarios = db.ListaComentarios.Where(a => a.cod_interno == id_to_string && a.tipo_nota == "CAMPANHA").ToList(),
                ListaEstagio = db.CampanhaMarketingEstagios.Where(a => a.campanhaId == id).OrderBy(p => p.num_seq).ToList(),
                ListaPgto = db.CampanhaMarketingPgto.Where(a => a.campanhaid == id).OrderBy(p => p.campanhamarketingpgtoid).ToList(),
                DadosDoCrm = db.Dados_crm.Where(a => a.cod_pessoa == Campanha.cod_pessoa_string).FirstOrDefault(),
                vlr_faturado = vlr_faturado,
                per_atingido = per_atingido,
                vlr_total_pago = db.CampanhaMarketingPgto.Where(a => a.campanhaid == id).Select(a => a.vlr_pgto).DefaultIfEmpty(0).Sum()
            };


            return View(data);
        }




        [HttpPost]
        public ActionResult RemoveDocFromCampanha(int id)
        {
            // Remove the item from the cart
            CampanhaMarketingDoc varDelete = db.CampanhaMarketingDoc.Where(a => a.CampanhaMarketingDocId == id).Single();
            db.CampanhaMarketingDoc.Remove(varDelete);

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



        public ActionResult Edit(int id)
        {
            CampanhaMarketing data = db.CampanhaMarketing.Find(id);
            data.vlr_custo_medio = data.vlr_custo_medio ?? 0;
            data.vlr_contrato = data.vlr_contrato ?? 0;
            data.vlr_meta = data.vlr_meta ?? 0;
            data.per_contrato = data.per_contrato ?? 0;
            
            

            if (data == null)
            {
                return InvokeHttpNotFound();
            }
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes.OrderBy(a => a.des_pessoa), "cod_pessoa", "des_pessoa", data.cod_representante);
            ViewBag.cod_pessoa = new SelectList(db.Ps_Pessoas.Where(a => a.cod_pessoa == data.cod_pessoa), "cod_pessoa", "des_pessoa", data.cod_pessoa);
            ViewBag.segmentoid = new SelectList(db.Segmentos, "segmentoid", "des_segmento", data.segmentoid);
            ViewBag.tipoacaoid = new SelectList(db.TipoAcao, "tipoacaoid", "des_acao", data.tipoacaoid);
            ViewBag.formapgtoid = new SelectList(db.FormaPgto, "formapgtoid", "des_forma", data.formapgtoid);

            
            return View(data);


        }


        // POST: /TipoLead/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueAdd")]
        [ParameterBasedOnFormName("delete", "isDelete")]
        public ActionResult Edit(CampanhaMarketing data, bool continueAdd, bool isDelete, FormCollection form, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes.OrderBy(a => a.des_pessoa), "cod_pessoa", "des_pessoa", data.cod_representante);
            ViewBag.cod_pessoa = new SelectList(db.Ps_Pessoas.Where(a => a.cod_pessoa == data.cod_pessoa), "cod_pessoa", "des_pessoa", data.cod_pessoa);
            ViewBag.segmentoid = new SelectList(db.Segmentos, "segmentoid", "des_segmento", data.segmentoid);
            ViewBag.tipoacaoid = new SelectList(db.TipoAcao, "tipoacaoid", "des_acao", data.tipoacaoid);
            ViewBag.formapgtoid = new SelectList(db.FormaPgto, "formapgtoid", "des_forma", data.formapgtoid);



            if (data.cod_pessoa == 0)
            {
                ModelState.AddModelError("", "Cliente deve ser informado");
                return View(data);
            }


            if (!isDelete)
            {
                if (ModelState.IsValid)
                {
                    var dados_da_pessoa = db.Dados_crm.Find(data.cod_pessoa.ToString());
                    if (dados_da_pessoa != null)
                    {
                        try
                        {
                            data.cod_regional = Convert.ToInt32(dados_da_pessoa.cod_gerente);
                        }
                        catch
                        {
                            data.cod_regional = 1001;
                        }


                        try
                        {
                            data.cod_diretoria = Convert.ToInt32(dados_da_pessoa.cod_diretoria);
                        }
                        catch
                        {
                            data.cod_diretoria = 2;
                        }


                        try
                        {
                            data.des_segmento = dados_da_pessoa.segmento;
                        }
                        catch
                        {
                            data.des_segmento = "ELETRICA";
                        }

                    }


                    data.dta_inicial = data.dta_inicial ?? null;
                    data.dta_final = data.dta_final ?? null;
                    data.cod_pessoa_string = data.cod_pessoa.ToString();
                    data.des_nome = data.des_nome.ToUpper().FormatToB2y();
                    data.cod_regional = Convert.ToInt32(dados_da_pessoa.cod_gerente);
                    data.dta_alteracao = System.DateTime.Now;


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


                                    int CampanhaDocId = db.Database.SqlQuery<Int32>("select CampanhaMarketingDoc_Seq.NextVal from dual ").FirstOrDefault<Int32>();
                                    string NomeArquivo = a.FileName; //data.campanhaID.ToString() + '_' + CampanhaDocId.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));

                                    //var path = Path.Combine(Server.MapPath("~/Arquivos/uploads"), NomeArquivo);


                                    CampanhaMarketingDoc doc = new CampanhaMarketingDoc
                                    {
                                        CampanhaMarketingDocId = CampanhaDocId,
                                        campanhaID = data.campanhaID,
                                        Caminho = NomeArquivo,
                                        des_contenttype = contentype,
                                        des_imagem = arqUp,
                                        nome_arquivo = NomeArquivo
                                    };
                                    db.CampanhaMarketingDoc.Add(doc);

                                    //a.SaveAs(path);



                                }
                            }
                        }
                    }


                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = data.campanhaID });
                }
                return View(data);
            }
            else
            {
                try
                {
                    CampanhaMarketing dataDelete = db.CampanhaMarketing.Find(data.campanhaID);
                    db.CampanhaMarketing.Remove(dataDelete);

                    var dataestagio = db.CampanhaMarketingEstagios.Where(a => a.campanhaId == data.campanhaID).ToList();
                    foreach (var sid in dataestagio)
                    {
                        db.CampanhaMarketingEstagios.Remove(sid);
                    }

                    var dataDoc = db.CampanhaMarketingDoc.Where(a => a.campanhaID == data.campanhaID).ToList();
                    foreach (var sid in dataDoc)
                    {
                        db.CampanhaMarketingDoc.Remove(sid);
                    }



                    db.SaveChanges();
                    RedirectToAction("WorkFlow");

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
                    return RedirectToAction("Edit", new { id = data.estagioId });
                }
                return RedirectToAction("List");
            }
        }



        public ActionResult Interagindo(int id,
          string Obs
          )
        {
            CampanhaMarketing data_update = db.CampanhaMarketing.Find(id);
            data_update.dta_alteracao = System.DateTime.Now;
            data_update.des_ult_obs = Obs;
            data_update.cod_usuario_alteracao = cd_usuario;

            CampanhaMarketingEstagios altera_linha = AddDataSaidaEstagioAnterior(data_update.campanhaID);
            CampanhaMarketingEstagios nova_linha = AddNovaLinhaTrocaEstagio(data_update.campanhaID, data_update.estagioId, data_update.estagioId, Obs);

            db.Entry(altera_linha).State = EntityState.Modified;
            db.Entry(data_update).State = EntityState.Modified;
            db.CampanhaMarketingEstagios.Add(nova_linha);

            try
            {
                db.SaveChanges();
                return Json(new { id = data_update.campanhaID, Msg = "Ok, Gerado OC com sucesso ", hasError = false });
            }
            catch (Exception e)
            {
                return Json(new { id = data_update.campanhaID, Msg = e.Message, hasError = true });
            }

        }


    }
}
