using CRM.Extends;
using CRM.Filters;
using Data.Context;
using Services.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRM.Controllers
{

     
    [iFilterLog, Authorize, HandleError] //, OutputCache(CacheProfile = "Mini")]
    public class BasePublicController : Controller
    {
        public BasePublicController()
        {

        }
          #region variavel
                protected b2yweb_entities db = null;
                protected Funcoes _Funcoes = new Funcoes();
                protected SendEmail _email = new SendEmail();
                protected int cd_empresa { get; set; }
                protected string consolida { get; set; }
                protected int cd_regional { get; set; }
                protected short cd_usuario { get; set; }
                protected int cd_gusuario { get; set; }
                protected string str_empresa { get; set; }
                protected string nome_usuario { get; set; }
                protected DateTime dt_atual_com_hora_sql { get; set; }
                protected DateTime dt_atual_sem_hora_sql { get; set; }
                protected List<int> list_regional = new List<int>();
                protected HttpCookie cookie;


        #endregion variavel


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            /*
            if (requestContext.HttpContext.Session["oRegional"] != null)
                list_regional = (List<int>)Session["oRegional"];


            if (requestContext.HttpContext.Session["cd_empresa"] != null)
                cd_empresa = requestContext.HttpContext.Session["cd_empresa"].ToString() == null ? 0 : Convert.ToInt32(requestContext.HttpContext.Session["cd_empresa"].ToString());
            if (requestContext.HttpContext.Session["cd_regional"] != null)
                cd_regional = requestContext.HttpContext.Session["cd_regional"].ToString() == null ? 0 : Convert.ToInt32(requestContext.HttpContext.Session["cd_regional"].ToString());
            if (requestContext.HttpContext.Session["cd_gusuario"] != null)
                cd_gusuario = requestContext.HttpContext.Session["cd_gusuario"].ToString() == null ? 0 : Convert.ToInt32(requestContext.HttpContext.Session["cd_gusuario"].ToString());
            if (requestContext.HttpContext.Session["str_empresa"] != null)
                str_empresa = requestContext.HttpContext.Session["str_empresa"].ToString() == null ? "DEMO" : requestContext.HttpContext.Session["str_empresa"].ToString();
            if (requestContext.HttpContext.Session["usuario"] != null)
                nome_usuario = requestContext.HttpContext.Session["usuario"].ToString() == null ? "" : requestContext.HttpContext.Session["usuario"].ToString();
            if (requestContext.HttpContext.Session["consolida"] != null)
                consolida = requestContext.HttpContext.Session["consolida"].ToString() == null ? "E" : requestContext.HttpContext.Session["consolida"].ToString();



            if (requestContext.HttpContext.Session["oEmpresa"] != null)
            {
                db = new b2yweb_entities(requestContext.HttpContext.Session["oEmpresa"].ToString());
            }
            else
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Clear();
            }


            */

            db = new b2yweb_entities("oracle");
            _email = new SendEmail();
            dt_atual_com_hora_sql = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dt_atual_sem_hora_sql = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            cookie = HttpContext.Request.Cookies.Get("PortalProcedimento");

          


            try
            {
                cd_usuario = Convert.ToInt16(cookie.Values["cd_usuario"].ToString());
            }
            catch
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Clear();
            }



        }



        protected virtual ActionResult InvokeHttpNotFound()
        {
             
            var routeData = new RouteData();

            routeData.Values.Add("controller", "Crm");
            routeData.Values.Add("action", "Error");
            routeData.Values.Add("area", "");


            //return RedirectToAction("Error", "Crm",  new { controller = "Crm", action = "Error", area = "" } );
            return RedirectToAction("Error");
        }

        public string TrataMsgErro(Exception error)
        {
            StackTrace trace = new StackTrace(error, true);
            string erroGerado = error.Message;
            int idNomeArqivo = trace.GetFrame(trace.FrameCount - 1).GetFileName().LastIndexOf('\\') + 1;
            string arquivo = trace.GetFrame(trace.FrameCount - 1).GetFileName().Substring(idNomeArqivo).ToString();
            string metodo = trace.GetFrame(trace.FrameCount - 1).GetMethod().Name;
            string linha = trace.GetFrame(trace.FrameCount - 1).GetFileLineNumber().ToString();
            string retorno = "<b> Erro Gerado: </b>" + erroGerado + "<br />" +
                "<b> Origem : </b>" + arquivo + "<br />" +
                "<b> Método: </b>" + metodo + "<br />" +
                "<b> linha: </b>" + linha + "<br />";

            return retorno;
        }


        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
