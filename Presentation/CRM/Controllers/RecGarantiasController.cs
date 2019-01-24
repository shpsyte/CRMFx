using CRM.App_Helpers;
using CRM.Extends;
using Domain.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class RecGarantiasController : BasePublicController
    {
        //
        // GET: /RecGarantias/
        string SQL_GET_GARANTIA_POR_REPRES = "  select a.cod_representante, b.des_pessoa des_representante,  a.garantiaid garantiaid , d.des_nome, a.cod_cliente, c.razao, a.dta_inclusao,  " +
                                            " a.num_nf_cliente, a.obs, a.vlr_garantia, a.num_coleta, a.dta_previsao_coleta, a.dta_coleta, a.obscoleta, a.volumes, a.especies " +
                                            "   from Garantia a " +
                                            "   Left Outer Join Representantes b on a.cod_representante = b.COD_PESSOA " +
                                            "   Left Outer Join Clientes c on c.CD_CADASTRO = a.cod_cliente " +
                                            "    Left Outer Join Ge_Status_Garantia d on d.cod_status = a.cod_status " +
                                            "   Where a.ind_emitido_nf = 'S' " +
                                                "   and a.Ind_Emitido_Coleta = 'S' " +
                                                "   and a.dta_finalizacao is null  " +
                                            "     and a.ind_cancelada = 'N' " +
                                                " ORDER BY A.garantiaid ";


        public ActionResult ListWithOrderCollection()
        {
            return View();
        }

        private double IntervaloDias(DateTime dataInicio, DateTime dataFim, DateTime dataIntermediaria)
        {
            long intervaloTotal = Math.Abs(dataFim.Ticks - dataInicio.Ticks);
            long intervaloInter = Math.Abs(dataIntermediaria.Ticks - dataInicio.Ticks);
            return intervaloInter * 100 / intervaloTotal;
        }


        public ActionResult ReadOrdersUsingSql(ParametersDataTable param)
        {

            //int total_de_linhas = db.IE_Itens_Vendas.AsNoTracking().Where(a => a.cod_cliente == id).Count();
            var allItem = db.Database.SqlQuery<V_Garantias_Representantes>(SQL_GET_GARANTIA_POR_REPRES);
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
                             des_representante = c.des_representante,
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
                             RepresentacaoTotal = Decimal.Round((c.vlr_garantia / Vlr_Total) * 100, 0)
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

        public ActionResult ReadItemJsUsingSql(ParametersDataTable param, int id)
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

        public ActionResult ReceiveOrder(int id)
        {
            int TotalDeItens = db.GarantiaItem.Where(a => a.garantiaid == id).Count();
            int TotalVerificado = db.GarantiaItem.Where(a => a.garantiaid == id).Where(c => c.conferido == "S").Count();
            int[] data = new int[] { id, TotalDeItens, TotalVerificado };
            return View(data);
        }

        //public class ViewModel
        //{
        //    public int CodeClass { get; set; } //class is reserved word
        //    public int TransferCount { get; set; }
        //}

        // http://localhost:57042/RecGarantias/ReceiveOrder/32958
        //[HttpPost]
        //public ActionResult GetData(IEnumerable<GarantiaItem> data, FormCollection f)
        //{
        //    return Json(new { Data = data });
        //}

        public ActionResult AddOrRemove(int garantiaId,
            int garantiaitemid,
            string cod_foxlux,
            int cod_item,
            bool isDelete,
            decimal qtd_recebida, decimal qtd_atendida,
            decimal qtd_fora_garantia, decimal qtd_outras_marcas,
            decimal qtd_faltante, decimal qtd_avariada, decimal qtd_descartada)
        {
            // GarantiaItem data_update = db.GarantiaItem.Find(garantiaId, garantiaitemid, cod_foxlux, cod_item);
            GarantiaItem data_update = db.GarantiaItem.Where(a => a.garantiaitemid == garantiaitemid).First();

            data_update.qtd_recebida = qtd_recebida;
            data_update.qtd_atendida = qtd_atendida;
            data_update.qtd_fora_garantia = qtd_fora_garantia;
            data_update.qtd_outras_marcas = qtd_outras_marcas;
            data_update.qtd_faltante = qtd_faltante;
            data_update.qtd_avariada = qtd_avariada;
            data_update.qtd_descartada = qtd_descartada;
            data_update.conferido = isDelete ? "N" : "S";

            db.Entry(data_update).State = EntityState.Modified;

            int TotalDeItens = db.GarantiaItem.Where(a => a.garantiaid == garantiaId).Count();
            int TotalVerificado = db.GarantiaItem.Where(a => a.garantiaid == garantiaId).Where(c => c.conferido == "S").Count();
            string asError = "ok_input";

            if ((qtd_atendida + qtd_fora_garantia + qtd_outras_marcas + qtd_faltante + qtd_avariada + qtd_descartada) != qtd_recebida)
            {
                asError = "error_input";
            }

            try
            {
                db.SaveChanges();
                TotalVerificado = db.GarantiaItem.Where(a => a.garantiaid == garantiaId).Where(c => c.conferido == "S").Count();

                return Json(new { Msg = "Item Atualizado com sucesso", QtdeGarantia = TotalDeItens, QtdeVerificada = TotalVerificado, CError = asError, IDCERROR = "qtd_atendida" + garantiaitemid.ToString() });
            }
            catch (Exception e)
            {
                return Json(new { Msg = e.Message, QtdeGarantia = TotalDeItens, QtdeVerificada = TotalVerificado, CError = "", IDCERROR = "" });

            }

        }


        public ActionResult FinalizaGarantia(int id)
        {

            decimal Tqtd_avariada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_avariada * p.vlr_lancamento);
            decimal Tqtd_faltante = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_faltante * p.vlr_lancamento);
            decimal Tqtd_outras_marcas = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_outras_marcas * p.vlr_lancamento);
            decimal Tqtd_atendida = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_atendida * p.vlr_lancamento);
            decimal Tqtd_descartada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_descartada * p.vlr_lancamento);
            decimal Tqtd_fora_garantia = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_fora_garantia * p.vlr_lancamento);
            decimal Tqtd_lancamento = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_lancamento * p.vlr_lancamento);
            decimal Ttd_reaproveitada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_reaproveitada * p.vlr_lancamento);
            decimal Ttd_recebida = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_recebida * p.vlr_lancamento);
            decimal Tqtd_rejeitada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_rejeitada * p.vlr_lancamento);

            decimal VlrCreditoCliente = Tqtd_atendida;
            decimal VlrDebitoCliente = Tqtd_fora_garantia + Tqtd_outras_marcas + Tqtd_faltante + Tqtd_avariada;
            decimal VlrDebitoFoxlux = Tqtd_descartada;
            decimal VlrDebitoRepresentante = 0;
            decimal VlrDebitoTransportador = 0;




            var data = new Garantia_Procedimento_Model
            {
                ProcedimentoAdm = new ProcedimentoAdm(),
                Garantia = db.Garantia.Find(id),
                vlr_credito_cliente = VlrCreditoCliente,
                vlr_debito_cliente = VlrDebitoCliente,
                vlr_debito_foxlux = VlrDebitoFoxlux,
                vlr_debito_representante = VlrDebitoRepresentante,
                vlr_debito_transportador = VlrDebitoTransportador,
                Obs = ""
            };

            decimal? VlrCreditoJaDadoAoCliente = data.Garantia.vlr_credito_vinculado;
            if (VlrCreditoJaDadoAoCliente.HasValue)
            {
                if (VlrCreditoJaDadoAoCliente > 0)
                {

                    if (VlrCreditoJaDadoAoCliente == VlrCreditoCliente)
                    {
                        data.Garantia.dta_finalizacao = System.DateTime.Now;
                        data.Garantia.cod_status = 3;
                        db.Entry(data.Garantia).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Details", "Garantias", new { id = data.Garantia.garantiaid });

                    }
                    else
                    if (VlrCreditoJaDadoAoCliente > VlrCreditoCliente)
                    {
                        data.vlr_credito_cliente = 0;
                        data.vlr_debito_representante = Math.Abs((decimal)VlrCreditoJaDadoAoCliente - (decimal)VlrCreditoCliente);
                    }
                    else
                    if (VlrCreditoJaDadoAoCliente < VlrCreditoCliente)
                    {
                        data.vlr_credito_cliente = Math.Abs((decimal)VlrCreditoJaDadoAoCliente - (decimal)VlrCreditoCliente);
                        data.vlr_debito_representante = 0;
                    }



                }


            }


            ViewBag.cd_cadastro = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == data.Garantia.cod_cliente).ToList(), "CD_CADASTRO", "RAZAO", data.Garantia.cod_cliente);
            ViewBag.cd_tipo = new SelectList(db.TP_PROCEDIMENTO.Where(a => a.ATIVO == "S").ToList(), "CD_TIPO", "DES_TIPO", string.Empty);
            ViewBag.cd_departamento = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S").ToList(), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", string.Empty);
            ViewBag.cod_transportador = new SelectList(db.TRANSPORTADOR.ToList(), "COD_TRANSPORTADOR", "FullNameWithCode", data.Garantia.cod_transportador);
            ViewBag.motivoid = new SelectList(db.Tp_Procedimento_Motivos.Where(a => a.COD_TIPO == 0).ToList(), "cod_tipo", "des_nome");




            return View(data);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult FinalizaGarantia(Garantia_Procedimento_Model data, IEnumerable<HttpPostedFileBase> files)
        {
            #region initializer
            ModelState.Clear();
            data.Garantia = db.Garantia.Find(data.Garantia.garantiaid);

            ViewBag.cd_cadastro = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == data.Garantia.cod_cliente).ToList(), "CD_CADASTRO", "RAZAO", data.Garantia.cod_cliente);
            ViewBag.cd_tipo = new SelectList(db.TP_PROCEDIMENTO.Where(a => a.ATIVO == "S").ToList(), "CD_TIPO", "DES_TIPO", data.ProcedimentoAdm.CD_TIPO);
            ViewBag.cd_departamento = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S").ToList(), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", data.ProcedimentoAdm.CD_DEPARTAMENTO);
            ViewBag.cod_transportador = new SelectList(db.TRANSPORTADOR.ToList(), "COD_TRANSPORTADOR", "FullNameWithCode", data.Garantia.cod_transportador);
            ViewBag.motivoid = new SelectList(db.Tp_Procedimento_Motivos.Where(a => a.COD_TIPO == 0).ToList(), "cod_tipo", "des_nome");
            data.ProcedimentoAdm.VL_CLIENTE = data.vlr_credito_cliente;
            data.ProcedimentoAdm.VL_DCLIENTE = data.vlr_debito_cliente;

            data.ProcedimentoAdm.VL_TRANSPORTADORA = data.vlr_debito_transportador;
            data.ProcedimentoAdm.VL_REPRESENTANTE = data.vlr_debito_representante;
            data.ProcedimentoAdm.VL_FOXLUX = data.vlr_debito_foxlux;


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

            #endregion

            if (ModelState.IsValid)
            {
                data.ProcedimentoAdm.NF_FOX = nf_fox;
                string dta_nf = "";
                string cod_oper = "";

                if (data.ProcedimentoAdm.NF_FOX != 0)
                {
                    dta_nf = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.DT_EMISSAO).FirstOrDefault().ToString();
                    cod_oper = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.COD_OPER).FirstOrDefault().ToString();
                    data.cd_transportador = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.CD_TRANSPORTADOR).FirstOrDefault();

                }
                else
                {
                    dta_nf = "";
                    cod_oper = "";
                    //  cod_transportador = 0;
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
                data.ProcedimentoAdm.CD_TRANSPORTADOR = data.cd_transportador;

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

                                //Int32? intCD_PROCEDIMENTOARQ = db.ProcedimentoAdmArq.Max(s => (Int32?)s.ID_ARQ);

                                //if (intCD_PROCEDIMENTOARQ != null)
                                //{
                                //    intCD_PROCEDIMENTOARQ++;
                                //}
                                //else
                                //{
                                //    intCD_PROCEDIMENTOARQ = 1;
                                //}
                                //string NomeArquivo = data.ProcedimentoAdm.CD_PROCEDIMENTO.ToString() + '_' + intCD_PROCEDIMENTOARQ.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));
                                //var path = Path.Combine(Server.MapPath(Settings.caminho_arquivos_procedimento), NomeArquivo);
                                //string sqlFile = string.Format(" INSERT INTO PROCEDIMENTOARQ VALUES ({0},{1},\'{2}\')",
                                //    intCD_PROCEDIMENTOARQ,
                                //    data.ProcedimentoAdm.CD_PROCEDIMENTO,
                                //    NomeArquivo);
                                //db.Database.ExecuteSqlCommand(sqlFile);
                                //a.SaveAs(path);
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






                try
                {
                    db.ProcedimentoAdm.Add(data.ProcedimentoAdm);
                    data.Garantia.dta_finalizacao = System.DateTime.Now;
                    data.Garantia.cod_status = 3;
                    data.Garantia.cod_procedimento_final = data.ProcedimentoAdm.CD_PROCEDIMENTO;
                    db.Entry(data.Garantia).State = EntityState.Modified;
                    GarantiaProcedimento Gatprocedimento = new GarantiaProcedimento { id = db.Database.SqlQuery<Int32>("select GarantiaProcedimentoSeq.NextVal from dual ").FirstOrDefault<Int32>(), garantiaId = data.Garantia.garantiaid, cod_procedimento = data.ProcedimentoAdm.CD_PROCEDIMENTO };
                    db.GarantiaProcedimento.Add(Gatprocedimento);

                    //sac
                    int cod_sac = 0;
                    try
                    {
                        cod_sac = db.SacGarantia.Where(a => a.garantiaId == data.Garantia.garantiaid).Select(p => p.cod_sac).FirstOrDefault();
                    }
                    catch { cod_sac = 0; }

                    if (cod_sac > 0)
                    {
                        PS_Sac data_sac_update = db.PS_Sac.Find(cod_sac);
                        data_sac_update.cod_situacao = int.Parse(Settings.cod_situacao_finalizacao);
                        db.Entry(data_sac_update).State = EntityState.Modified;

                    }




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


            return RedirectToAction("Details", "Garantias", new { id = data.Garantia.garantiaid });



            return View(data);
        }




        public ActionResult VinculaGarantia(int id)
        {

            decimal Tqtd_avariada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_avariada * p.vlr_lancamento);
            decimal Tqtd_faltante = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_faltante * p.vlr_lancamento);
            decimal Tqtd_outras_marcas = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_outras_marcas * p.vlr_lancamento);
            decimal Tqtd_atendida = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_atendida * p.vlr_lancamento);
            decimal Tqtd_descartada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_descartada * p.vlr_lancamento);
            decimal Tqtd_fora_garantia = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_fora_garantia * p.vlr_lancamento);
            decimal Tqtd_lancamento = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_lancamento * p.vlr_lancamento);
            decimal Ttd_reaproveitada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_reaproveitada * p.vlr_lancamento);
            decimal Ttd_recebida = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_recebida * p.vlr_lancamento);
            decimal Tqtd_rejeitada = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.qtd_rejeitada * p.vlr_lancamento);
            decimal Timpostos = db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => p.vlr_icms_subs + p.vlr_icms_subs);

            decimal VlrCreditoCliente = Tqtd_atendida + Timpostos;
            decimal VlrDebitoCliente = Tqtd_fora_garantia + Tqtd_outras_marcas + Tqtd_faltante + Tqtd_avariada;
            decimal VlrDebitoFoxlux = Tqtd_descartada;
            decimal VlrDebitoRepresentante = 0;
            decimal VlrDebitoTransportador = 0;

            



            var data = new Garantia_Procedimento_Model
            {
                ProcedimentoAdm = new ProcedimentoAdm(),
                Garantia = db.Garantia.Find(id),
                vlr_credito_cliente = VlrCreditoCliente,
                vlr_debito_cliente = VlrDebitoCliente,
                vlr_debito_foxlux = VlrDebitoFoxlux,
                vlr_debito_representante = VlrDebitoRepresentante,
                vlr_debito_transportador = VlrDebitoTransportador,
                Obs = ""
            };

            ViewBag.cd_cadastro = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == data.Garantia.cod_cliente).ToList(), "CD_CADASTRO", "RAZAO", data.Garantia.cod_cliente);
            ViewBag.cd_tipo = new SelectList(db.TP_PROCEDIMENTO.Where(a => a.ATIVO == "S").ToList(), "CD_TIPO", "DES_TIPO", string.Empty);
            ViewBag.cd_departamento = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S").ToList(), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", string.Empty);
            ViewBag.cod_transportador = new SelectList(db.TRANSPORTADOR.ToList(), "COD_TRANSPORTADOR", "FullNameWithCode", data.Garantia.cod_transportador);
            ViewBag.motivoid = new SelectList(db.Tp_Procedimento_Motivos.Where(a => a.COD_TIPO == 0).ToList(), "cod_tipo", "des_nome");
            return View(data);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult VinculaGarantia(Garantia_Procedimento_Model data, IEnumerable<HttpPostedFileBase> files)
        {
            #region initializer
            ModelState.Clear();
            data.Garantia = db.Garantia.Find(data.Garantia.garantiaid);

            ViewBag.cd_cadastro = new SelectList(db.Clientes.Where(a => a.CD_CADASTRO == data.Garantia.cod_cliente).ToList(), "CD_CADASTRO", "RAZAO", data.Garantia.cod_cliente);
            ViewBag.cd_tipo = new SelectList(db.TP_PROCEDIMENTO.Where(a => a.ATIVO == "S").ToList(), "CD_TIPO", "DES_TIPO", data.ProcedimentoAdm.CD_TIPO);
            ViewBag.cd_departamento = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S").ToList(), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", data.ProcedimentoAdm.CD_DEPARTAMENTO);
            ViewBag.cod_transportador = new SelectList(db.TRANSPORTADOR.ToList(), "COD_TRANSPORTADOR", "FullNameWithCode", data.Garantia.cod_transportador);
            ViewBag.motivoid = new SelectList(db.Tp_Procedimento_Motivos.Where(a => a.COD_TIPO == 0).ToList(), "cod_tipo", "des_nome");

            data.ProcedimentoAdm.VL_CLIENTE = data.vlr_credito_cliente;
            data.ProcedimentoAdm.VL_DCLIENTE = data.vlr_debito_cliente;

            data.ProcedimentoAdm.VL_TRANSPORTADORA = data.vlr_debito_transportador;
            data.ProcedimentoAdm.VL_REPRESENTANTE = data.vlr_debito_representante;
            data.ProcedimentoAdm.VL_FOXLUX = data.vlr_debito_foxlux;


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

            #endregion






            if (ModelState.IsValid)
            {
                data.ProcedimentoAdm.NF_FOX = nf_fox;
                string dta_nf = "";
                string cod_oper = "";




                if (data.ProcedimentoAdm.NF_FOX != 0)
                {
                    dta_nf = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.DT_EMISSAO).FirstOrDefault().ToString();
                    cod_oper = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.COD_OPER).FirstOrDefault().ToString();
                    data.cd_transportador = (from a in db.eNota.Where(a => a.NR_NOTA == data.ProcedimentoAdm.NF_FOX) select a.CD_TRANSPORTADOR).FirstOrDefault();
                }
                else
                {
                    dta_nf = "";
                    cod_oper = "";
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
                data.ProcedimentoAdm.CD_TRANSPORTADOR = data.cd_transportador;

                if (files != null)
                {
                    foreach (var a in files)
                    {
                        if (a != null)
                        {
                            if (a.ContentLength > 0)
                            {
                                Int32? intCD_PROCEDIMENTOARQ = db.ProcedimentoAdmArq.Max(s => (Int32?)s.ID_ARQ);

                                if (intCD_PROCEDIMENTOARQ != null)
                                {
                                    intCD_PROCEDIMENTOARQ++;
                                }
                                else
                                {
                                    intCD_PROCEDIMENTOARQ = 1;
                                }
                                string NomeArquivo = data.ProcedimentoAdm.CD_PROCEDIMENTO.ToString() + '_' + intCD_PROCEDIMENTOARQ.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));
                                var path = Path.Combine(Server.MapPath(Settings.caminho_arquivos_procedimento), NomeArquivo);
                                string sqlFile = string.Format(" INSERT INTO PROCEDIMENTOARQ VALUES ({0},{1},\'{2}\')",
                                    intCD_PROCEDIMENTOARQ,
                                    data.ProcedimentoAdm.CD_PROCEDIMENTO,
                                    NomeArquivo);
                                db.Database.ExecuteSqlCommand(sqlFile);
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


                try
                {
                    db.ProcedimentoAdm.Add(data.ProcedimentoAdm);

                    data.Garantia.cod_procedimento_vinculado = data.ProcedimentoAdm.CD_PROCEDIMENTO;
                    data.Garantia.vlr_credito_vinculado = data.ProcedimentoAdm.VL_CLIENTE;
                    db.Entry(data.Garantia).State = EntityState.Modified;

                    GarantiaProcedimento Gatprocedimento = new GarantiaProcedimento { id = db.Database.SqlQuery<Int32>("select GarantiaProcedimentoSeq.NextVal from dual ").FirstOrDefault<Int32>(), garantiaId = data.Garantia.garantiaid, cod_procedimento = data.ProcedimentoAdm.CD_PROCEDIMENTO };
                    db.GarantiaProcedimento.Add(Gatprocedimento);


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


            return RedirectToAction("Details", "Garantias", new { id = data.Garantia.garantiaid });



            return View(data);
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
        [HttpPost]
        public ActionResult ConfirmaRecGarantia(int garantiaId, string Obs)
        {
            // verifica se existe algum itm sem conferencia
            bool existe_item_sem_conferencia = db.GarantiaItem.Where(a => a.garantiaid == garantiaId && a.conferido == "N").Count() > 0;

            if (existe_item_sem_conferencia)
            {
                return Json(new { id = garantiaId, Msg = "Existem Itens sem conferência, verifique", hasError = true });
            }
            else
            {
                EnvaiEmailDivergencia(garantiaId, Obs);

                return Json(new { id = garantiaId, Msg = "", hasError = false });
            }


        }


       // [HttpPost]
        public void EnvaiEmailDivergencia(int id, string Obs)
        {
            List<GarantiaItem> itenscondivergencia = new List<GarantiaItem>();
            decimal TdtDivergencia = 0;
            foreach (var item in db.GarantiaItem.Where(a => a.garantiaid == id))
            {
                decimal Tqtd_atendida = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_atendida);
                decimal Tqtd_lancamento = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_lancamento);

                decimal Tqtd_avariada = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_avariada);
                decimal Tqtd_faltante = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_faltante);
                decimal Tqtd_outras_marcas = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_outras_marcas);
                decimal Tqtd_descartada = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_descartada);
                decimal Tqtd_fora_garantia = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_fora_garantia);
                decimal Ttd_reaproveitada = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_reaproveitada);
                decimal Ttd_recebida = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_recebida);
                decimal Tqtd_rejeitada = db.GarantiaItem.Where(a => a.garantiaid == id && a.garantiaitemid == item.garantiaitemid).Sum(p => p.qtd_rejeitada);

                TdtDivergencia = Tqtd_avariada + Tqtd_faltante + Tqtd_outras_marcas + Tqtd_descartada + Tqtd_fora_garantia
                    + Ttd_reaproveitada + Ttd_recebida + Tqtd_rejeitada;


                if (Tqtd_lancamento != TdtDivergencia)
                {
                    itenscondivergencia.Add(item);
                }


            }


            if (itenscondivergencia.Any())
            {
                string[] _to = new string[] { ConfigurationManager.AppSettings["EmailNFDivergenciaDev"].ToString() };

                //envia email se a garantia estiver com divergencia
                SendEmail email = new SendEmail();
                email.EnviarEmailDivergenciaGarantia(id, "DivergenciaGarantia.htm", _to, itenscondivergencia);
            }
                
            //    return Json(new { Msg = "Enviado ok" });
            //}
            //else
            //{
            //    return Json(new { Msg = "Não enviado" });

            //}


        }

        [ValidateInput(false)]
        public ActionResult GetImgItemGat(int id, int garantiaitemId, int cod_item)
        {
            ViewData["Garantia"] = id;
            ViewData["GarantiaItemId"] = garantiaitemId;
            ViewData["cod_item"] = cod_item;


            var data = db.GarantiaImagens.Where(p => p.garantiaId == id && p.cod_item == cod_item).ToList();

            return View(data);
        }


        public ActionResult ViewImg(int id)
        {
            var imageData = db.GarantiaImagens.Where(a => a.id == id).Select(p => p.picture).First();
            return File(imageData, "image/jpg");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveImagens(int id, int garantiaitemId, int cod_item)
        {
            for (int i = 0; i <= Request.Files.Count - 1; i++)
            {
                int tamanho = (int)Request.Files[i].InputStream.Length;
                string contentype = Request.Files[i].ContentType;
                byte[] arq = new byte[tamanho];

                Request.Files[i].InputStream.Read(arq, 0, tamanho);
                byte[] arqUp = arq;

                int nextt = db.Database.SqlQuery<Int32>("select GarantiaImagensSeq.NextVal from dual ").FirstOrDefault<Int32>();
                string NomeArquivo = garantiaitemId.ToString() + '_' + nextt.ToString() + '_' + string.Format("{0}", Path.GetFileName(Request.Files[i].FileName));


                GarantiaImagens cat = new GarantiaImagens
                {
                    id = nextt,
                    garantiaId = id,
                    garantiaitemid = garantiaitemId,
                    cod_item = cod_item,
                    picture = arqUp,
                    des_contenttype = contentype,
                    nome_arquivo = Request.Files[i].FileName

                };

                db.GarantiaImagens.Add(cat);
            }



            //if (files != null)
            //{
            //    foreach (var a in files)
            //    {
            //        if (a != null)
            //        {
            //            if (a.ContentLength > 0)
            //            {
            //                Int32? intCD_PROCEDIMENTOARQ = db.ProcedimentoAdmArq.Max(s => (Int32?)s.ID_ARQ);

            //                if (intCD_PROCEDIMENTOARQ != null)
            //                {
            //                    intCD_PROCEDIMENTOARQ++;
            //                }
            //                else
            //                {
            //                    intCD_PROCEDIMENTOARQ = 1;
            //                }
            //                string NomeArquivo = data.ProcedimentoAdm.CD_PROCEDIMENTO.ToString() + '_' + intCD_PROCEDIMENTOARQ.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));
            //                var path = Path.Combine(Server.MapPath("~/Arquivos/uploads"), NomeArquivo);
            //                string sqlFile = string.Format(" INSERT INTO PROCEDIMENTOARQ VALUES ({0},{1},\'{2}\')",
            //                    intCD_PROCEDIMENTOARQ,
            //                    data.ProcedimentoAdm.CD_PROCEDIMENTO,
            //                    NomeArquivo);
            //                db.Database.ExecuteSqlCommand(sqlFile);
            //                a.SaveAs(path);
            //            }
            //        }
            //    }
            //}


            try
            {
                db.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>  window.close() ;</script>");
            }
            catch (Exception e)
            {
                return JavaScript(e.Message);
            }

        }


        public ActionResult Download(int id)
        {
            var docs = (from a in db.GarantiaImagens
                        where a.id == id
                        select new
                        {
                            doc = a.picture,
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
