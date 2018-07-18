using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;



namespace CRM.Filters
{
    public class iFilterLog : ActionFilterAttribute
    {
       

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Session["IP"] = System.Web.HttpContext.Current.Request.UserHostAddress;
             
            
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                FormsAuthentication.SignOut();
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Protected", action = "Login", area = "" }));
            }
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