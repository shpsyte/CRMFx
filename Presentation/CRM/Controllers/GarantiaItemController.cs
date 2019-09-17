using CRM.App_Helpers;
using CRM.Extends;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class GarantiaItemController : BasePublicController
    {
        //
        // GET: /GarantiaItem/
        int _take = 1000;

        string SQLBASE = "SELECT * FROM " +
                            " (select b.num_seq, a.cod_cliente, c.des_pessoa, b.cod_item, ie_cod_completo_sp(b.cod_item, '107') Cod_Foxlux, d.des_item, b.dta_lancamento, " +
                            "        trunc(sysdate) - b.dta_lancamento Dias, b.qtd_lancamento, b.num_documento, b.vlr_total, cast(b.vlr_total / b.qtd_lancamento as number(18, 4))Vlr_unitario, b.vlr_ipi, b.vlr_icms_subs " +
                            "  from Ns_Notas a " +
                            " Inner " +
                            "  join Ce_Diarios b on a.cod_emp = b.cod_emp and a.num_seq = b.num_seq_ns " +
                            " Inner join Ps_Pessoas c on c.cod_pessoa = a.cod_cliente " +
                            " Inner Join Ie_itens d on d.cod_item = b.cod_item " +
                            " where b.TIP_OPERACAO = 1 " +
                            " and b.TIP_LANCAMENTO = 2 " +
                            " ORDER BY num_seq DESC)  Tmp Where NUM_SEQ >= {0} and rownum <= {1} ";

        string SQLBASEPREENCHECLASS = "SELECT * FROM " +
                            " (select a.cod_unidade, b.num_seq, a.cod_cliente, c.des_pessoa, b.cod_item, ie_cod_completo_sp(b.cod_item, '107') Cod_Foxlux, d.des_item, b.dta_lancamento, " +
                            "        trunc(sysdate) - b.dta_lancamento Dias, b.qtd_lancamento, cast( b.num_documento as varchar2(20) ) num_documento , b.vlr_total, cast(b.vlr_total / b.qtd_lancamento as number(18, 4)) Vlr_unitario, " +
                            " cast(b.vlr_ipi / b.qtd_lancamento as number(18, 4)) vlr_ipi , cast(b.vlr_icms_subs / b.qtd_lancamento as number(18, 4)) vlr_icms_subs , " +
                            " cast(b.vlr_icms / b.qtd_lancamento as number(18, 4)) vlr_icms, cast(b.vlr_base_subs / b.qtd_lancamento as number(18, 4)) vlr_base_subs " +
                            " , e.per_icms picms, Round(((b.vlr_ipi / b.vlr_total ) * 100),2) pipi, "+
                            " Round(((b.vlr_icms_subs / b.vlr_total ) * 100),2) picmsst, " +
                            //" round((((b.vlr_base_subs / (b.vlr_total + b.vlr_ipi) ) - 1) *100),2) mvast  " +
                            " round(nvl((e.idx_subs - 1)*100,0),2) mvast  " +
                            "  from Ns_Notas a " +
                            " Inner " +
                            "  join Ce_Diarios b on a.cod_emp = b.cod_emp and a.num_seq = b.num_seq_ns " +
                            " Inner join Ps_Pessoas c on c.cod_pessoa = a.cod_cliente " +
                            " Inner Join Ie_itens d on d.cod_item = b.cod_item " +
                            "    Inner Join Ns_Itens e on e.num_seq = b.num_seq_ns and e.cod_item = b.cod_item and e.num_seq_ce = b.num_seq " +
                            " where b.TIP_OPERACAO = 1  " +
                            " and b.TIP_LANCAMENTO = 2    " +
                            " and a.cod_unidade <> 3010  " +
                            " and c.cod_pessoa = {0} " +
                            " and b.dta_lancamento >= \'{1}\' " +
                            " order by b.dta_lancamento DESC )  Tmp Where COD_CLIENTE = {2} and rownum <= 1000 ";

        string SQLBASECOUNT = " SELECT count(*) " +
                            "      FROM Ns_Notas a INNER JOIN Ce_Diarios b on a.cod_emp = b.cod_emp and a.num_seq = b.num_seq_ns " +
                            "     WHERE b.TIP_OPERACAO = 1 AND b.TIP_LANCAMENTO = 2 AND a.cod_cliente = {0} ";

        public ActionResult AddItem(int id, int cod_cliente)
        {
            string ssql = string.Format(" DELETE FROM CartItem WHERE GarantiaId = {0} ", id);
            db.Database.ExecuteSqlCommand(ssql);
            //var data = db.IE_Itens_Vendas.Where(p => p.cod_cliente == cod_cliente).OrderByDescending(p => p.dta_lancamento).OrderBy(a => a.cod_foxlux).ToList();
            int[] data = new int[] { id, cod_cliente };
            return View(data);
        }

        public ActionResult ReadItemJsUsingSql(ParametersDataTable param, int id)
        {
            //int total_de_linhas = db.IE_Itens_Vendas.AsNoTracking().Where(a => a.cod_cliente == id).Count();
            int total_de_linhas = db.Database.SqlQuery<int>(string.Format(SQLBASECOUNT, id)).FirstOrDefault();


            //IEnumerable<IE_Itens_Vendas> filteredCompanies;
            //var allitems2 = db.IE_Itens_Vendas.AsNoTracking().Where(p => p.cod_cliente == id).ToList();
            IEnumerable<V_IE_Itens_Vendas> filteredCompanies;

            DateTime? dtaFaturamento = null;
            if (Request["sSearch_1"].ToString() != "") dtaFaturamento = Convert.ToDateTime(Request["sSearch_1"]);
            var codfoxlux = Convert.ToString(Request["sSearch_2"]);
            var desItem = Convert.ToString(Request["sSearch_3"]);
            var numNota = Convert.ToString(Request["sSearch_5"]);

            var isDataFaturamento = dtaFaturamento != null;
            var iscodfoxlux = !string.IsNullOrEmpty(codfoxlux);
            var isdesItem = !string.IsNullOrEmpty(desItem);
            var isnumNota = !string.IsNullOrEmpty(numNota);
            bool hasSearch = (isDataFaturamento != false || iscodfoxlux != false || isdesItem != false || isnumNota != false);

            //if (hasSearch) { _take = int.MaxValue; } else { _take = 700; };
            _take = 1000;



            //if (iscodfoxlux) filteredCompanies = filteredCompanies.Where(p => p.cod_foxlux.ToUpper().Contains(codfoxlux.ToUpper()));
            //if (isdesItem) filteredCompanies = filteredCompanies.Where(p => p.des_item.ToUpper().Contains(desItem.ToUpper()));
            //if (isnumNota) filteredCompanies = filteredCompanies.Where(p => p.num_documento.ToUpper().Contains(numNota.ToUpper()));
            //if (isDataFaturamento) filteredCompanies = filteredCompanies.Where(p => p.dta_lancamento == dtaFaturamento);

            if (iscodfoxlux) SQLBASEPREENCHECLASS += string.Format(" AND COD_FOXLUX LIKE ('%{0}%')", codfoxlux.ToUpper().Trim());
            if (isdesItem) SQLBASEPREENCHECLASS += string.Format(" AND DES_ITEM LIKE ('%{0}%')", desItem.ToUpper().Trim());
            if (isnumNota) SQLBASEPREENCHECLASS += string.Format(" AND NUM_DOCUMENTO LIKE ('%{0}%')", numNota.ToUpper().Trim());
            if (isDataFaturamento) SQLBASEPREENCHECLASS += string.Format(" AND DTA_LANCAMENTO = \'{0}\'", dtaFaturamento.Value.ToShortDateString());

            var dta_an = System.DateTime.Now.AddYears(-2).ToShortDateString();
            NLS_SETTINGS.alter_session_nl(db);

            var Item = db.Database.SqlQuery<V_IE_Itens_Vendas>(string.Format(SQLBASEPREENCHECLASS, id, dta_an, id));


            /*var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<V_IE_Itens_Vendas, string> orderingFunction = (c =>
                                                              sortColumnIndex == 0 ? c.dta_lancamento.ToString() :
                                                              sortColumnIndex == 1 ? c.cod_foxlux :
                                                              sortColumnIndex == 2 ? c.des_item :
                                                              sortColumnIndex == 4 ? c.num_documento.ToString() :
                                                               c.dta_lancamento.ToString()
                                                            );
                                                            */

            filteredCompanies = Item;


            filteredCompanies = filteredCompanies.Take(_take);
            

            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);
            


            
            var result = from c in displayedCompanies.ToList()
                         select new
                         {
                             num_seq = c.num_seq,
                             dta_lancamento = c.dta_lancamento.ToShortDateString(),
                             cod_foxlux = c.cod_foxlux,
                             cod_item = c.cod_item,
                             des_item = c.des_item,
                             qtd_lancamento = c.qtd_lancamento.ToString(),
                             num_documento = c.num_documento.ToString(),
                             vlr_unitario = c.vlr_unitario.ToString("C"),
                             //vlr_unitario = c.vlr_unitario.ToString("#.####"),
                             vlr_total = c.vlr_total.ToString("C"),

                             vlr_ipi = c.vlr_ipi.ToString("C"),
                             vlr_icms_subs = c.vlr_icms_subs.ToString("C"),
                             vlr_icms = c.vlr_icms.ToString("C"),
                             vlr_base_subs = c.vlr_base_subs.ToString("C"),

                             dias = c.dias.ToString(),
                             QtdeNova = c.qtd_lancamento.ToString(),
                             Check = 0.ToString(),
                             Img = "",
                             Garantia = c.dias >= 545 ? "N" : "S",
                             ObsItem = "",
                             Fator =  "1",

                             picms = c.picms,
                             pipi = c.pipi,
                             picmsst = c.picmsst,
                             mvast = c.mvast,
                             cod_unidade = c.cod_unidade
                         };

            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total_de_linhas,
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result.ToList()
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;
        }

        public ActionResult ReadItemJsUsingSqlAndView(ParametersDataTable param, int id)
        {
            int total_linhas = db.Database.SqlQuery<int>(string.Format(SQLBASECOUNT, id)).FirstOrDefault();

            IEnumerable<IE_Itens_Vendas> filteredCompanies;

            /*var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<IE_Itens_Vendas, string> orderingFunction = (c =>
                                                              sortColumnIndex == 0 ? c.dta_lancamento.ToString() :
                                                              sortColumnIndex == 1 ? c.cod_foxlux :
                                                              sortColumnIndex == 2 ? c.des_item :
                                                              sortColumnIndex == 4 ? c.num_documento.ToString() :
                                                               c.dta_lancamento.ToString()
                                                            );
                                                            */


            DateTime? dtaFaturamento = null;
            if (Request["sSearch_0"].ToString() != "")
            {
                dtaFaturamento = Convert.ToDateTime(Request["sSearch_0"]);

            }
            var codfoxlux = Convert.ToString(Request["sSearch_1"]);
            var desItem = Convert.ToString(Request["sSearch_2"]);
            var numNota = Convert.ToString(Request["sSearch_4"]);

            //Optionally check whether the columns are searchable at all 

            var isDataFaturamento = dtaFaturamento != null;
            var iscodfoxlux = !string.IsNullOrEmpty(codfoxlux);
            var isdesItem = !string.IsNullOrEmpty(desItem);
            var isnumNota = !string.IsNullOrEmpty(numNota);
            bool hasSearch = (isDataFaturamento != false || iscodfoxlux != false || isdesItem != false || isnumNota != false);
            //if (hasSearch) _take = int.MaxValue;

            filteredCompanies = db.IE_Itens_Vendas.AsNoTracking().Where(c => c.cod_cliente == id);

            if (iscodfoxlux) filteredCompanies = filteredCompanies.Where(p => p.cod_foxlux.ToUpper().Contains(codfoxlux.ToUpper()));
            if (isdesItem) filteredCompanies = filteredCompanies.Where(p => p.des_item.ToUpper().Contains(desItem.ToUpper()));
            if (isnumNota) filteredCompanies = filteredCompanies.Where(p => p.num_documento.ToUpper().Contains(numNota.ToUpper()));
            //if (isDataFaturamento) filteredCompanies = filteredCompanies.Where(p => p.dta_lancamento == dtaFaturamento);

            filteredCompanies = filteredCompanies.Take(_take);
            filteredCompanies = filteredCompanies.OrderByDescending(p => p.dta_lancamento).ToList();

            var displayedCompanies = filteredCompanies
                             .Skip(param.iDisplayStart)
                             .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new string[] {
                             //"View",
                             c.dta_lancamento.ToShortDateString(),
                             c.cod_foxlux,
                             c.des_item,
                             c.qtd_lancamento.ToString(),
                             c.num_documento.ToString(),
                             c.vlr_unitario.ToString("C"),
                             c.vlr_total.ToString("C"),
                             c.vlr_ipi.ToString("C"),
                             c.vlr_icms_subs.ToString("C"),
                             c.dias.ToString()

                         };


            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total_linhas,
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;
        }

        public ActionResult ReadItemJsUsingView(ParametersDataTable param, int id)
        {
            int total_linhas = db.IE_Itens_Vendas.AsNoTracking().Where(a => a.cod_cliente == id).Count();


            IEnumerable<IE_Itens_Vendas> filteredCompanies;

            /*var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<IE_Itens_Vendas, string> orderingFunction = (c =>
                                                              sortColumnIndex == 0 ? c.dta_lancamento.ToString() :
                                                              sortColumnIndex == 1 ? c.cod_foxlux :
                                                              sortColumnIndex == 2 ? c.des_item :
                                                              sortColumnIndex == 4 ? c.num_documento.ToString() :
                                                               c.dta_lancamento.ToString()
                                                            );
                                                            */
            if ((!string.IsNullOrEmpty(param.sSearch)))
            {
                filteredCompanies = db.IE_Itens_Vendas.AsNoTracking()
                   .Where(c => c.cod_cliente == id
                   && (c.cod_foxlux.ToUpper().Contains(param.sSearch.ToUpper())
                        || c.num_documento.ToUpper().Contains(param.sSearch.ToUpper())
                      ));
            }
            else
            {
                filteredCompanies = db.IE_Itens_Vendas.AsNoTracking().Where(a => a.cod_cliente == id).Take(_take);
            }

            /* var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);
                */

            filteredCompanies = filteredCompanies.OrderByDescending(c => c.dta_lancamento);

            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new string[] {
                             //"View",
                             c.dta_lancamento.ToShortDateString(),
                             c.cod_foxlux,
                             c.des_item,
                             c.qtd_lancamento.ToString(),
                             c.num_documento.ToString(),
                             c.vlr_unitario.ToString("C"),
                             c.vlr_total.ToString("C"),
                             c.vlr_ipi.ToString("C"),
                             c.vlr_icms_subs.ToString("C"),
                             c.dias.ToString()

                         }.ToList();


            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total_linhas,
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;
        }

        public ActionResult ReadItemJss(ParametersDataTable param, int id)
        {
            int allItem = db.IE_Itens_Vendas.Where(a => a.cod_cliente == id).Count();


            string OrderyBy = "";
            string WhereClause = string.Format(" AND 1=1 AND COD_CLIENTE = {0} ", id.ToString());
            string sql = string.Format(SQLBASE, param.iDisplayStart, param.iDisplayLength);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            switch (sortColumnIndex)
            {
                case 0:
                    OrderyBy = " ORDER BY COD_FOXLUX ";
                    break;
                case 1:
                    OrderyBy = " ORDER BY DES_ITEM  ";
                    break;
                case 2:
                    OrderyBy = " ORDER BY DIAS  ";
                    break;
                case 3:
                    OrderyBy = " ORDER BY QTD_LANCAMENTO  ";
                    break;
                case 4:
                    OrderyBy = " ORDER BY DTA_LANCAMENTO  ";
                    break;
                case 5:
                    OrderyBy = " ORDER BY NUM_DOCUMENTO  ";
                    break;
                default:
                    OrderyBy = " ORDER BY COD_ITEM ";
                    break;
            }

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                OrderyBy += " ASC ";
            else
                OrderyBy += " DESC ";

            OrderyBy = " ORDER BY DTA_LANCAMENTO DESC ";


            if ((!string.IsNullOrEmpty(param.sSearch)))
            {
                string pesquisa = param.sSearch.ToUpper();
                WhereClause += " AND (";
                WhereClause += string.Format(" COD_FOXLUX LIKE ('%{0}%') ", pesquisa);
                WhereClause += string.Format(" OR  COD_ITEM LIKE ('%{0}%') ", pesquisa);
                WhereClause += string.Format(" OR  COD_ITEM LIKE ('%{0}%') ", pesquisa);
                WhereClause += string.Format(" OR  DES_ITEM LIKE ('%{0}%') ", pesquisa);
                WhereClause += string.Format(" OR  NUM_DOCUMENTO LIKE ('%{0}%') ", pesquisa);
                WhereClause += ")";

            }


            sql += WhereClause + " " + OrderyBy;
            var FilteredCompany = db.Database.SqlQuery<V_IE_Itens_Vendas>(sql).ToList();

            var result = from c in FilteredCompany
                         select new string[] {
                             //"View",
                             c.cod_foxlux,
                             c.des_item,
                             c.qtd_lancamento.ToString(),
                             c.num_documento.ToString(),
                             c.vlr_unitario.ToString("C"),
                             c.vlr_total.ToString("C"),
                             c.vlr_ipi.ToString("C"),
                             c.vlr_icms_subs.ToString("C"),
                             c.dias.ToString(),
                             c.dta_lancamento.ToShortDateString()
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allItem,
                iTotalDisplayRecords = allItem,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }



        public ActionResult AddOrRemove(int garantiaId, int cod_cliente, string cod_foxlux, int cod_item,  bool isDelete, decimal qtde, int num_nota, decimal vlr_unitario
            , decimal vlr_ipi, decimal vlr_icms_subs, decimal vlr_icms, decimal vlr_base_subs, string obsitem, decimal fator,
            decimal picms, decimal pipi, decimal pcimsst, decimal mvast, int cod_unidade

            )
        {
            //int sessao = db.Database.SqlQuery<int>(" SELECT USERENV('SESSIONID') FROM DUAL ").FirstOrDefault();
            int NextRecordId = db.Database.SqlQuery<int>(" SELECT CartItemSeq.NextVal FROM DUAL ").FirstOrDefault();
            string Msg = "";


            if (!isDelete)
            {
                CartItem NewItem = new CartItem()
                {
                    garantiaId = garantiaId,
                    cod_Foxlux = cod_foxlux.TrimStart().TrimEnd(),
                    cod_item = cod_item,
                    num_nota = num_nota,
                    qtde_Garantia = qtde,
                    recordId = NextRecordId,
                    vlr_Unitario = vlr_unitario / fator,
                    vlr_icms = (vlr_icms / fator) * qtde,
                    vlr_icms_subs = (vlr_icms_subs / fator) * qtde,
                    vlr_ipi = (vlr_ipi / fator) * qtde,
                    vlr_base_subs = (vlr_base_subs / fator) * qtde,
                    ObsItem = obsitem,
                    picms = picms,
                    pipi = pipi,
                    picmsst = pcimsst,
                    mvast = mvast ,
                    cod_unidade = cod_unidade

                };
                db.CartItem.Add(NewItem);
            }
            else
            {
                int RecordId = db.CartItem.Where(a => a.cod_Foxlux == cod_foxlux && a.garantiaId == garantiaId && a.num_nota == num_nota).Select(a => a.recordId).FirstOrDefault();
                CartItem DeleteItem = db.CartItem.Find(garantiaId, RecordId, cod_foxlux, cod_item, num_nota);

                if (DeleteItem != null)
                {
                    db.CartItem.Remove(DeleteItem);
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


            var _itens = db.CartItem.Where(a => a.garantiaId == garantiaId).ToList();
            var results = new
            {
                Msg = Msg,
                CartTotal = _itens.Select(a => a.qtde_Garantia * a.vlr_Unitario).Sum(),
                CartCount = _itens.Count()
            };
            return Json(results);

        }


        public PartialViewResult GetDetailsFromDoc(int garantiaId, int cod_cliente)
        {
            string sql = "";
            sql = String.Format(" SELECT A.COD_FOXLUX COD_ITEM, B.DES_ITEM,  "+
                                " A.QTDE_GARANTIA QTD_NEGOCIADA, A.VLR_UNITARIO VLR_UNI_BRUTO, A.QTDE_GARANTIA * A.VLR_UNITARIO VLR_TOTAL " +
                                " FROM  CARTITEM A INNER JOIN IE_ITENS  B ON A.COD_ITEM = B.COD_ITEM " +
                                " WHERE A.GARANTIAID = {0} ", garantiaId);
            var Itens = db.Database.SqlQuery<GetItensDoc>(sql).ToList();
            ViewData["Garantia"] = garantiaId;
            return PartialView(Itens);
        }

        [HttpPost]
        public ActionResult ConfirmItens(int garantiaId, int cod_cliente)
        {
            var AllItem = db.CartItem.Where(a => a.garantiaId == garantiaId).ToList();

            if (AllItem == null)
                return RedirectToAction("Details", "Garantias", new { id = garantiaId });



            foreach (var a in AllItem)
            {
                int NextRecordId = db.Database.SqlQuery<int>(" SELECT GARANTIAITEM_SEQ.NextVal FROM DUAL ").FirstOrDefault();
                GarantiaItem NewItem = new GarantiaItem()
                {
                    cod_foxlux = a.cod_Foxlux.Trim().TrimEnd().TrimStart(),
                    caminho_foto = null,
                    cod_item = a.cod_item,
                    garantiaid = a.garantiaId,
                    garantiaitemid = NextRecordId,
                    qtd_lancamento = a.qtde_Garantia,
                    vlr_total = a.vlr_Unitario * a.qtde_Garantia,
                    num_nota = a.num_nota,
                    obs = "",
                    justificativa =  a.ObsItem,
                    qtd_avariada = 0,
                    qtd_faltante = 0,
                    qtd_outras_marcas = 0,
                    qtd_atendida = a.qtde_Garantia,
                    qtd_descartada = 0,
                    qtd_fora_garantia = 0,
                    qtd_reaproveitada = 0,
                    qtd_recebida = 0,
                    qtd_rejeitada = 0,
                    conferido = "N",
                    vlr_lancamento = a.vlr_Unitario,
                    vlr_ipi = a.vlr_ipi,
                    vlr_icms = a.vlr_icms,
                    vlr_base_subs = a.vlr_base_subs,
                    vlr_icms_subs = a.vlr_icms_subs,
                    picms = a.picms,
                    pipi = a.pipi,
                    picmsst = a.picmsst,
                    mvast = (a.mvast > 0 ? a.mvast : 0),
                    cod_unidade = a.cod_unidade
                    
                };
                db.GarantiaItem.Add(NewItem);
                
            }


            //altera e coloca o total da garantia 

            try
            {
                db.SaveChanges();
                AtualizaTotalGarantia(garantiaId);
                return Json(new { id = garantiaId, Msg = "Ok, Redirecioando para detalhes...", hasError = false });
            }
            catch (Exception e)
            {
                return Json(new { id = garantiaId, Msg = e.Message, hasError = true });
            }

        }

        private decimal AtualizaTotalGarantia(int garantiaId)
        {
            var vlr_garantia = db.GarantiaItem.Where(a => a.garantiaid == garantiaId).Sum(p => (decimal?)p.vlr_total) ?? 0;
            var vlr_ipi = db.GarantiaItem.Where(a => a.garantiaid == garantiaId).Sum(p => (decimal?)p.vlr_ipi) ?? 0;
            var vlr_st = db.GarantiaItem.Where(a => a.garantiaid == garantiaId).Sum(p => (decimal?)p.vlr_icms_subs) ?? 0;

            Garantia DataFromUpdate = db.Garantia.Find(garantiaId);
            DataFromUpdate.vlr_garantia = vlr_garantia + vlr_ipi + vlr_st;
            db.Entry(DataFromUpdate).State = EntityState.Modified;
            db.SaveChanges();

            return db.GarantiaItem.Where(a => a.garantiaid == garantiaId).Sum(p => (decimal?)p.vlr_total) ?? 0;

        }

        [HttpPost]
        public ActionResult RemoveItemGarantia(int id)
        {
            // Remove the item from the cart
            GarantiaItem varDelete = db.GarantiaItem.Where(p => p.garantiaitemid == id).Single();
            db.GarantiaItem.Remove(varDelete);

            try
            {
                db.SaveChanges();
                decimal total = AtualizaTotalGarantia(varDelete.garantiaid);
                return Json(new { id = id, Msg = "Ok, Atualizado item com suceso ", total = total, hasError = false });
            }
            catch (Exception e)
            {
                return Json(new { id = id, Msg = e.Message, total = GetTotalGarantia(varDelete.garantiaid), hasError = true });
            }


        }

        private decimal GetTotalGarantia(int id)
        {
            return db.GarantiaItem.Where(a => a.garantiaid == id).Sum(p => (decimal?)p.vlr_total) ?? 0;
        }
    }
}
