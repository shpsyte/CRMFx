using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;



namespace CRM.Filters
{
    public class iFilterRelatorioMkt : ActionFilterAttribute
    {
        public int sessao;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var db = new b2yweb_entities("oracle");
            filterContext.Controller.ViewBag.ano = new SelectList((from e in db.CampanhaMarketing
                                                                   group e by e.dta_inclusao.Year into g
                                                                   select new { Year = g.Key, Events = g }), "Year", "Year", "2017");

            filterContext.Controller.ViewBag.statusId = new SelectList(db.Status, "statusId", "descricao", "3");
            filterContext.Controller.ViewBag.segmentoId = new SelectList(db.Segmentos, "segmentoid", "des_segmento");
            filterContext.Controller.ViewBag.regionalid = new SelectList(db.Regional.OrderBy(a => a.CD_REGIONAL), "CD_REGIONAL", "DESCRICAO");
            filterContext.Controller.ViewBag.tipoacaoId = new SelectList(db.TipoAcao, "segmentoid", "des_acao");
            //filterContext.Controller.ViewBag.pessoaId = new SelectList(db.Clientes, "CD_CADASTRO", "RAZAO");


            //sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();
            base.OnActionExecuting(filterContext);
        }

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    string empresa = filterContext.HttpContext.Session["str_empresa"].ToString();
        //    db = new ERPEntities(empresa);


        //   // Executará esse codigo antes de entrar numa Action/Controller
        //   filterContext.Controller.ViewBag.lista_cliente        = _FuncoesERP.ReadCadastroVinculado();
        //   filterContext.Controller.ViewBag.lista_vendedores     = _FuncoesERP.ReadComissionadoVinculado();
        //   filterContext.Controller.ViewBag.lista_tppedido       = _FuncoesERP.ReadTipoPedidoVenda();
        //   filterContext.Controller.ViewBag.lista_stpedido       = _FuncoesERP.ReadSituacaoPedidoVenda();
        //   filterContext.Controller.ViewBag.lista_estagiopedido  = _FuncoesERP.EstagioPedido();


        //    //varias consutlas para colocar em ViewBag

        //    base.OnActionExecuting(filterContext);
        //}

    }
}