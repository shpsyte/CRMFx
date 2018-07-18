using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.Ajax.Utilities;
using Services.Functions;
using Data.Context;
using Domain.Entity;
using System.Linq;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Collections.Generic;

namespace CRM.App_Helpers
{

    public static class HtmlHelperExtensions
    {
        private static string _displayVersion;


        public static IHtmlString TextBoxForCreateOrDetails<TModel, TProperty>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression,
        object htmlAttributes, bool disabled
    )
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (disabled)
            {
                attributes["disabled"] = "disabled";
            }
            return htmlHelper.TextBoxFor(expression, attributes);
        }

        /// <summary>
        ///     Retrieves a non-HTML encoded string containing the assembly version as a formatted string.
        ///     <para>If a project name is specified in the application configuration settings it will be prefixed to this value.</para>
        ///     <para>
        ///         e.g.
        ///         <code>1.0 (build 100)</code>
        ///     </para>
        ///     <para>
        ///         e.g.
        ///         <code>ProjectName 1.0 (build 100)</code>
        ///     </para>
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IHtmlString AssemblyVersion(this HtmlHelper helper)
        {
            if (_displayVersion.IsNullOrWhiteSpace())
                SetDisplayVersion();

            return helper.Raw(_displayVersion);
        }

        /// <summary>
        ///     Compares the requested route with the given <paramref name="value" /> value, if a match is found the
        ///     <paramref name="attribute" /> value is returned.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value">The action value to compare to the requested route action.</param>
        /// <param name="attribute">The attribute value to return in the current action matches the given action value.</param>
        /// <returns>A HtmlString containing the given attribute value; otherwise an empty string.</returns>
        public static IHtmlString RouteIf(this HtmlHelper helper, string value, string attribute)
        {
            var currentController =
                (helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();
            var currentAction =
                (helper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty).ToString().UnDash();

            var hasController = value.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            return hasAction || hasController ? new HtmlString(attribute) : new HtmlString(string.Empty);
        }

        public static IHtmlString RouteIf(this HtmlHelper helper, string value, string attribute, string controllers)
        {
            var currentController =
                (helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();

            var currentAction =
                (helper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty).ToString().UnDash();


            var hasController = value.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var listaControllers = controllers.Split(';');
            if (!String.IsNullOrEmpty(controllers))
            {
                hasController = listaControllers.Contains(currentController);
            }




            var hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            return hasAction || hasController ? new HtmlString(attribute) : new HtmlString(string.Empty);
        }


        public static IHtmlString RouteIfControllerAction(this HtmlHelper helper, string contoller, string action, string attribute)
        {
            var listaControllers = contoller.Split(';');
            var listaAction = action.Split(';');


            var currentController =
                (helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();

            var currentAction =
                (helper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty).ToString().UnDash();



            var hasController = listaControllers.Contains(currentController); //contoller.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var hasAction = listaAction.Contains(currentAction); //action.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);



            return hasAction && hasController ? new HtmlString(attribute) : new HtmlString(string.Empty);
        }


        public static IHtmlString ReturnNoteName(this HtmlHelper helper, string tipo, string cod_interno, string msg, string usuario)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");
            string collBase = " Postou um Comentário ";
            string Fantasia = "";
            string CdCadatro = "";
            string url = "";


            if (tipo.Equals("ACCOUNT"))
            {
                Fantasia = db.Dados_crm.Where(a => a.cod_pessoa == cod_interno).Select(a => a.des_pessoa).FirstOrDefault();
                CdCadatro = db.Dados_crm.Where(a => a.cod_pessoa == cod_interno).Select(a => a.cod_pessoa).FirstOrDefault();
                url = "/Account/ViewProfile/" + CdCadatro;
                collBase += " na conta ";
            }

            if (tipo.Equals("LEADS"))
            {
                int id;
                try
                {
                    id = Convert.ToInt32(cod_interno);
                }
                catch
                {
                    id = 0;
                }

                Fantasia = db.Ps_Leads.Where(a => a.cod_lead == id).Select(a => a.des_nome).FirstOrDefault();
                int cdLead = Convert.ToInt32(db.Ps_Leads.Where(a => a.cod_lead == id).Select(a => a.cod_lead).FirstOrDefault());
                url = "/Lead/Edit/" + cdLead.ToString();
                collBase += " no lead ";
            }


            if (tipo.Equals("GERAL"))
            {
                Fantasia = " Geral ";
                CdCadatro = "";
                url = "#";
            }

            if (tipo.Equals("SAC"))
            {
                int id;
                try
                {
                    id = Convert.ToInt32(cod_interno);
                }
                catch
                {
                    id = 0;
                }

                Fantasia = db.PS_Sac.Where(a => a.cod_sac == id).Select(a => a.PS_Pessoas_Sac != null ? a.PS_Pessoas_Sac.des_pessoa : a.des_nome != null ? a.des_nome : "Não atribuído").FirstOrDefault();
                int cdLead = Convert.ToInt32(db.PS_Sac.Where(a => a.cod_sac == id).Select(a => a.cod_sac).FirstOrDefault());
                url = "/Sac/Details/" + cdLead.ToString();
                collBase += " no sac de: ";
            }

            if (tipo.Equals("GARANTIA"))
            {
                int id;
                try
                {
                    id = Convert.ToInt32(cod_interno);
                }
                catch
                {
                    id = 0;
                }

                Fantasia = db.Garantia.Where(a => a.garantiaid == id).Select(a => a.Ps_Pessoas.des_pessoa).FirstOrDefault();
                url = "/Garantias/Details/" + id.ToString();
                collBase += " na Garantia  de: ";
            }


            if (tipo.Equals("CAMPANHA"))
            {
                int id;
                try
                {
                    id = Convert.ToInt32(cod_interno);
                }
                catch
                {
                    id = 0;
                }

                Fantasia = db.CampanhaMarketing.Where(a => a.campanhaID == id).Select(a => a.des_nome).FirstOrDefault();
                url = "/CampanhaMarketing/Details/" + id.ToString();
                collBase += " na Campanha de: ";
            }

            var sb = new StringBuilder();
            sb.AppendFormat(collBase + "<a href={0}>{1}</a>", url, Fantasia);

            return new HtmlString(sb.ToString());
        }



        public static IHtmlString AddTextIf(this HtmlHelper helper, bool condition, string ValueTrue, string ValueFalse)
        {

            return condition ? new HtmlString(ValueTrue) : new HtmlString(ValueFalse);
        }

        public static IHtmlString QtdeProcedimentoAberto(this HtmlHelper helper)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");

            var exceptionList = new List<Int32> { 2, 3, 4 };

            string _qtde = (from a in db.ProcedimentoAdm
                            where !exceptionList.Contains(a.ID_SITUACAO)
                            select a).Count().ToString();

            return new HtmlString(_qtde);
        }


        public static IHtmlString QtdeSac(this HtmlHelper helper, string tipo)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");
            var exceptionList = new List<Int32> { 2, 3 };
            var exceptionList2 = new List<Int32> { 2, 3, 41 };

            string _qtde = "0";


            switch (tipo)
            {
                case "Aberto":
                    _qtde = (from a in db.PS_Sac where !exceptionList.Contains((int)a.cod_situacao) select a).Count().ToString();
                    break;
                case "Fechado":
                    _qtde = (from a in db.PS_Sac where exceptionList.Contains((int)a.cod_situacao) select a).Count().ToString();
                    break;
                case "Andamento":
                    _qtde = (from a in db.PS_Sac where a.cod_situacao == 41 select a).Count().ToString();
                    break;
                case "Atendimento":
                    _qtde = (from a in db.PS_Sac where !exceptionList2.Contains((int)a.cod_situacao) select a).Count().ToString();
                    break;
            }



            return new HtmlString(_qtde);
        }


        public static IHtmlString ValoresGat(this HtmlHelper helper, int id, int nota, string tipo)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");
            IEnumerable<GarantiaItem> _itens;

            int qt = db.CartItemPrint.Where(a => a.garantiaId == id).Select(a => a.garantiaId).Count();
            if (qt == 0)
            {
                _itens = db.GarantiaItem.Where(p => p.garantiaid == id && p.num_nota == (nota > 0 ? nota : p.num_nota)).ToList();
            }
            else
            {


                _itens = (from t1 in db.GarantiaItem
                          join t2 in db.CartItemPrint
                           on new { A = t1.cod_foxlux, B = t1.cod_item, C = t1.garantiaid } equals new { A = t2.cod_Foxlux, B = t2.cod_item, C = t2.garantiaId }
                          where t1.garantiaid == id && t1.num_nota == (nota > 0 ? nota : t1.num_nota)
                          select t1).ToList();
            }






            decimal? _valor = decimal.Zero;

            switch (tipo)
            {
                case "P":
                    _valor = _itens.Sum(P => (decimal?)P.vlr_total);
                    break;
                case "ICMS":
                    _valor = _itens.Sum(P => (decimal?)P.vlr_icms);
                    break;
                case "IPI":
                    _valor = _itens.Sum(P => (decimal?)P.vlr_ipi);
                    break;
                case "BICMSST":
                    _valor = _itens.Sum(P => (decimal?)P.vlr_base_subs);
                    break;
                case "ICMSST":
                    _valor = _itens.Sum(P => (decimal?)P.vlr_icms_subs);
                    break;
                case "TOTAL":
                    _valor = _itens.Sum(p => (decimal?)p.vlr_total + (decimal?)p.vlr_ipi + (decimal?)p.vlr_icms_subs);
                    break;

            }





            return new HtmlString(_valor.Value.ToString("c"));
        }

        public static IHtmlString QtdepedidoGerente(this HtmlHelper helper)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");

            int _qt = db.Database.SqlQuery<Int32>("Select count(*) from pe_pedidos where cod_situacao = 65  ").FirstOrDefault<Int32>();

            return new HtmlString(_qt.ToString());
        }
        public static IHtmlString QtdepedidoFaturar(this HtmlHelper helper)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");

            int _qt = db.Database.SqlQuery<Int32>(" Select count(*) from pe_pedidos where cod_situacao = 355  ").FirstOrDefault<Int32>();

            return new HtmlString(_qt.ToString());
        }



        public static IHtmlString QtdeGarantiaAreceberAberto(this HtmlHelper helper)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");

            string _qtde = db.Garantia.Where(p => p.ind_emitido_coleta == "S" && p.ind_cancelada == "N" && p.dta_finalizacao == null).Count().ToString();


            return new HtmlString(_qtde);
        }

        public static IHtmlString QtdeSacAberto(this HtmlHelper helper)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");

            var exceptionList = new List<Int32?> { 2, 3 };

            string _qtde = (from a in db.PS_Sac
                            where !exceptionList.Contains(a.cod_situacao)
                            select a).Count().ToString();


            return new HtmlString(_qtde);
        }


        public static IHtmlString GetDescItem(this HtmlHelper helper, string cod_foxlux)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");
            return new HtmlString(db.IE_Itens.Where(a => a.cod_foxlux == cod_foxlux).Select(a => a.des_item).First());
        }

        public static decimal? ValorGatAbertoTransito(this HtmlHelper helper)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");
            decimal? valor_aberto_transito = db.Garantia.Where(a => a.ind_emitido_nf == "S" && a.dta_finalizacao == null && a.ind_cancelada == "N").Select(uf => uf.vlr_garantia).DefaultIfEmpty().Sum();
            //string retorno = valor_aberto_transito.HasValue ? valor_aberto_transito.Value.ToString("c") : "0";

            return valor_aberto_transito;
        }


        public static IHtmlString QtdeFeedsMes(this HtmlHelper helper)
        {
            b2yweb_entities db = new b2yweb_entities("oracle");
            int Mes = System.DateTime.Now.Month;
            int Ano = System.DateTime.Now.Year;

            string _qtde = db.ListaComentarios.Where(a => a.dta_inclusao.Month == Mes && a.dta_inclusao.Year == Ano).Count().ToString();
            return new HtmlString(_qtde);
        }


        public static IHtmlString FormataCnpj(this HtmlHelper helper, string Value)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                try
                {


                    if (Value.Length == 14)
                    {
                        string formatado = Convert.ToUInt64(Value).ToString(@"00\.000\.000\/0000\-00");
                        return new HtmlString(formatado);
                    }
                    else if (Value.Length == 11)
                    {
                        string formatado = Convert.ToUInt64(Value).ToString(@"000\.000\.000\-00");
                        return new HtmlString(formatado);
                    }
                    else
                    {
                        return new HtmlString(Value);
                    }
                }
                catch (Exception)
                {
                    return new HtmlString(Value);
                }
            }
            return new HtmlString("");


        }
        /// <summary>
        ///     Renders the specified partial view with the parent's view data and model if the given setting entry is found and
        ///     represents the equivalent of true.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="appSetting">The key value of the entry point to look for.</param>
        public static void RenderPartialIf(this HtmlHelper htmlHelper, string partialViewName, string appSetting)
        {
            var setting = Settings.GetValue<bool>(appSetting);

            htmlHelper.RenderPartialIf(partialViewName, setting);
        }

        /// <summary>
        ///     Renders the specified partial view with the parent's view data and model if the given setting entry is found and
        ///     represents the equivalent of true.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="condition">The boolean value that determines if the partial view should be rendered.</param>
        public static void RenderPartialIf(this HtmlHelper htmlHelper, string partialViewName, bool condition)
        {
            if (!condition)
                return;

            htmlHelper.RenderPartial(partialViewName);
        }

        /// <summary>
        ///     Retrieves a non-HTML encoded string containing the assembly version and the application copyright as a formatted
        ///     string.
        ///     <para>If a company name is specified in the application configuration settings it will be suffixed to this value.</para>
        ///     <para>
        ///         e.g.
        ///         <code>1.0 (build 100) © 2015</code>
        ///     </para>
        ///     <para>
        ///         e.g.
        ///         <code>1.0 (build 100) © 2015 CompanyName</code>
        ///     </para>
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IHtmlString Copyright(this HtmlHelper helper)
        {
            var copyright =
                string.Format("{0} &copy; {1} {2}", helper.AssemblyVersion(), DateTime.Now.Year, Settings.Company)
                    .Trim();

            return helper.Raw(copyright);
        }

        private static void SetDisplayVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            _displayVersion =
                string.Format("{4} {0}.{1}.{2} (build {3})", version.Major, version.Minor, version.Build,
                    version.Revision, Settings.Project).Trim();
        }

        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that utilizes bootstrap markup and styling.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="alertType">The alert type styling rule to apply to the summary element.</param>
        /// <param name="heading">The optional value for the heading of the summary element.</param>
        /// <returns></returns>
        public static HtmlString ValidationBootstrap(this HtmlHelper htmlHelper, string alertType = "danger",
            string heading = "")
        {
            if (htmlHelper.ViewData.ModelState.IsValid)
                return new HtmlString(string.Empty);

            var sb = new StringBuilder();

            sb.AppendFormat("<div class=\"alert alert-{0} alert-block\">", alertType);
            sb.Append("<button class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>");

            if (!heading.IsNullOrWhiteSpace())
            {
                sb.AppendFormat("<h4 class=\"alert-heading\">{0}</h4>", heading);
            }

            sb.Append(htmlHelper.ValidationSummary());
            sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }


        public static HtmlString FirstLettersOfname(this HtmlHelper htmlHelper, string Name)
        {
            var sb = new StringBuilder();
            string firstLetters = "";

            foreach (var part in Name.Split(' '))
            {
                try
                {
                    firstLetters += part.Substring(0, 1);
                }
                catch
                {
                    firstLetters += "";
                }
            }

            firstLetters = firstLetters.Substring(0, 2);
            string css = "border-radius: 0;position: relative;border: 5px solid #fff;top: -30px;left: 10px;display: inline-block;text-align: right;z-index: 4;max-width: 100px;margin-bottom: -30px;";
            css += "min-width: 120px;min-height: 120px;text-align: center;padding: 35px;color: #fff;font-weight: 900;font-size: 25px;background-color: rgb(221, 128, 128);";


            sb.AppendFormat("<div style='{0}'> {1}", css, firstLetters);
            sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }
    }


}