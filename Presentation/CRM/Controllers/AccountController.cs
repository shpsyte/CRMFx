using CRM.App_Helpers;
using CRM.Models;
using Domain.Entity;
using Services.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
namespace CRM.Controllers
{
    [CustomAuthorize(AccessLevel = "Admin;User")]
    public class AccountController : BasePublicController
    {
        //
        // GET: /Empresa/
        public ActionResult ReadAccount()
        {
            return View();
        }
        public ActionResult ReadAccountJs(ParametersDataTable param)
        {
            int total_de_linhas = db.Database.SqlQuery<int>(" SELECT COUNT(*) FROM dados_crm").FirstOrDefault();
            string SQLBASEPREENCHECLASS = " select a.cod_pessoa, a.des_pessoa, a.des_represetante, a.tel_contato, " +
                " a.des_email_cli, a.dta_ult_compra,  cast(sysdate - a.dta_ult_compra as int) daysNotSale , a.vlr_faturamento  from dados_crm a Where 1=1 ";


            IEnumerable<V_Crm> filteredCompanies;
            if ((!string.IsNullOrEmpty(param.sSearch)))
            {
                string search = param.sSearch.ToUpper().Trim();

                SQLBASEPREENCHECLASS += string.Format(" and ( a.des_pessoa LIKE ('%{0}%')", search);
                SQLBASEPREENCHECLASS += string.Format(" or a.des_fantasia LIKE ('%{0}%')", search);
                SQLBASEPREENCHECLASS += string.Format(" or a.des_represetante LIKE ('%{0}%')", search);
                SQLBASEPREENCHECLASS += string.Format(" or a.cod_pessoa = \'{0}\' ) ", search);
                //SQLBASEPREENCHECLASS += string.Format(" or a.tel_contato LIKE ('%{0}%') )", search);

            }


            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            switch (sortColumnIndex)
            {
                case 0:
                    SQLBASEPREENCHECLASS += " order by a.cod_pessoa ";
                    break;
                case 1:
                    SQLBASEPREENCHECLASS += " order by a.des_pessoa ";
                    break;
                case 2:
                    SQLBASEPREENCHECLASS += " order by a.des_representante ";
                    break;
                case 3:
                    SQLBASEPREENCHECLASS += " order by a.tel_contato ";
                    break;
                case 4:
                    SQLBASEPREENCHECLASS += " order by a.dta_ult_compra ";
                    break;
                case 5:
                    SQLBASEPREENCHECLASS += " order by a.sysdate - a.dta_ult_compra  ";
                    break;
                case 6:
                    SQLBASEPREENCHECLASS += " order by a.vlr_faturamento ";
                    break;

                default:
                    SQLBASEPREENCHECLASS += " order by a.cod_pessoa ";
                    break;
            }

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                SQLBASEPREENCHECLASS += " asc ";
            else
                SQLBASEPREENCHECLASS += " desc ";



            var allitem = db.Database.SqlQuery<V_Crm>(SQLBASEPREENCHECLASS);

            filteredCompanies = allitem;
            filteredCompanies = filteredCompanies.Take(1000);

            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength).ToList();



            var result = from c in displayedCompanies
                         select new string[]
                         {
                             Convert.ToString(c.cod_pessoa),
                             c.des_pessoa,
                             c.des_represetante,
                             c.tel_contato,
                             c.dta_ult_compra.HasValue  ? c.dta_ult_compra.Value.ToShortDateString() :  c.dta_ult_compra.ToString(),
                             c.daysNotSale.HasValue  ? c.daysNotSale.Value.ToString() :  "0",
                             c.vlr_faturamento.ToString("C")
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
        public ActionResult ViewProfile(string id)
        {



            int Accountid;
            try
            {
                Accountid = Convert.ToInt32(id);
            }
            catch { Accountid = 0; }
            List<ListaClientesDaColigada> ListaClientes = null;
            if (Accountid == 0)
            {
                ListaClientes = (from a in db.Dados_crm
                                 where a.cod_grupo == id
                                 select new ListaClientesDaColigada { 
                                     cod_pessoa = a.cod_pessoa
                                    ,des_pessoa = a.des_pessoa
                                    ,vlr_faturamento = a.vlr_faturamento.HasValue ? (decimal)a.vlr_faturamento.Value : 0
                                 }).ToList();
            }
            DateTime dt_ano = System.DateTime.Now.AddYears(-1);

            


            var model = new DadosDoCrmModel
            {
                DadosDoCrm = db.Dados_crm.Find(id),
                Listapedidos = db.Lista_Pedidos.Where(a => a.cod_cliente == Accountid && Accountid > 0 && a.dta_emissao >= dt_ano).OrderByDescending(a => a.dta_emissao).ToList(),
                ListaComentarios = db.ListaComentarios.Where(a => a.cod_interno == id && a.tipo_nota == "ACCOUNT" ).ToList(),
                Listanotafisccais = db.Lista_Notas.Where(a => a.cod_cliente == Accountid && Accountid > 0 && a.dta_emissao >= dt_ano).OrderByDescending(a => a.dta_emissao).ToList(),
                ListaTitulos = db.Lista_Titulos.Where(a => a.cod_pessoa == Accountid && Accountid > 0).OrderBy(a => a.diasatraso).ToList(),
                ListaOcorrencia = db.Lista_Ocorrencia.Where(a => a.cod_pessoa.Equals(id) && Accountid > 0).OrderBy(a => a.dta_abertura).ToList(),
                PerfilCrm = db.Ps_Perfil_Crm.Find(id),
                NomeUsuario = cookie["usuario"],
                IsColigada = Accountid == 0,
                ListaClientesDaColigada = ListaClientes,
                ListaContato = db.Contatos.Where(p => p.cod_conta == id).ToList()
            };
            


            return View(model);
        }
        [HttpPost]
        public JsonResult PostInfo(string pk, string name, string value)
        {
            if (1 == 1)
            {
                var AsInfo = db.Ps_Perfil_Crm.Find(pk);
                if (AsInfo == null)
                {
                    string sql = string.Format(" INSERT INTO Ps_Perfil_Crm VALUES (\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\',\'{5}\')",
                            pk,
                            "",
                            "",
                            "",
                            "",
                            ""
                            );
                    db.Database.ExecuteSqlCommand(sql);
                    AsInfo = db.Ps_Perfil_Crm.Find(pk);
                }
                PropertyInfo propertyInfo = AsInfo.GetType().GetProperty(name);
                if (propertyInfo != null)
                {
                    Type t = propertyInfo.PropertyType;
                    object d;
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (String.IsNullOrEmpty(value))
                            d = null;
                        else
                            d = Convert.ChangeType(value, t.GetGenericArguments()[0]);
                    }
                    else if (t == typeof(Guid))
                    {
                        d = new Guid(value);
                    }
                    else
                    {
                        d = Convert.ChangeType(value, t);
                    }
                    propertyInfo.SetValue(AsInfo, d, null);
                }
                //AsInfo.des_acao = value;
                db.Entry(AsInfo).State = EntityState.Modified;
                db.SaveChanges();
            }
            List<Object> Adicionado = new List<Object>();
            Adicionado.Add(new
                    {
                        Sucesso = true
                    });
            return Json(Adicionado);
        }
        public PartialViewResult GetDetailsFromDoc(int Accountid, string num_pedido, string cod_compl, string tipo)
        {
            string sql = "";
            if (tipo.Equals("pe_pedidos"))
            {
                sql = String.Format(" SELECT IE_COD_COMPLETO_SP(A.COD_ITEM,'107') COD_ITEM, B.DES_ITEM, " +
                                    " A.QTD_NEGOCIADA, A.VLR_UNI_BRUTO, A.QTD_NEGOCIADA * A.VLR_UNI_BRUTO VLR_TOTAL " +
                                    "    FROM  PE_ITENS A INNER JOIN IE_ITENS  B ON A.COD_ITEM = B.COD_ITEM " +
                                    "    WHERE A.NUM_PEDIDO = {0} " +
                                    "      AND A.COD_COMPL = {1} ", num_pedido, cod_compl);
            }
            else
                if (tipo.Equals("ns_notas"))
                {
                    sql = String.Format(" SELECT IE_COD_COMPLETO_SP(A.COD_ITEM,'107') COD_ITEM, B.DES_ITEM, " +
                                        " A.QTD_LANCAMENTO qtd_negociada, ROUND(A.VLR_TOTAL /  A.QTD_LANCAMENTO,4) VLR_UNI_BRUTO, A.VLR_TOTAL VLR_TOTAL " +
                                       " FROM  CE_DIARIOS A INNER JOIN IE_ITENS  B ON A.COD_ITEM = B.COD_ITEM " +
                                       " INNER JOIN NS_NOTAS C ON C.COD_EMP = A.COD_EMP AND C.NUM_SEQ = A.NUM_SEQ_NS " +
                                       " WHERE C.NUM_NOTA = {0}", num_pedido);
                };
            var Itens = db.Database.SqlQuery<GetItensDoc>(sql).ToList();
            if (tipo == "pe_pedidos")
            {
                ViewData["Tipo"] = "Pedido ";
                ViewData["Pedido"] = String.Concat(num_pedido, "-", cod_compl);
            }
            else
            {
                ViewData["Tipo"] = "Nota Fiscal ";
                ViewData["Pedido"] = String.Concat(num_pedido);
            }
            return PartialView(Itens);
        }
    }
}
