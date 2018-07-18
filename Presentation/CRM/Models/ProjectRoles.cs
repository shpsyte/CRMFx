using Data.Context;
using Services.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Models
{
    public static class ProjectRoles
    {
        //now constants for the attribute values
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Guest = "Guest";

    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();


        //Property to allow array instead of single string.
        private string[] _authorizedRoles;
        public string[] AuthorizedRoles
        {
            get { return _authorizedRoles ?? new string[0]; }
            set { _authorizedRoles = value; }
        }

        public string AccessLevel { get; set; }
        public string Roles { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //If its an unauthorized/timed out ajax request go to top window and redirect to logon.
            //            if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAjaxRequest())
            //               filterContext.Result = new JavaScriptResult() 
            //             { Script = top.location = "/Account/LogOn?Expired=1" };

            //If authorization results in HttpUnauthorizedResult, redirect to error page instead of Logon page.
            if (filterContext.Result is HttpUnauthorizedResult)
                filterContext.Result = new RedirectResult("~/Protected/Login"); // ("~/Error/Authorization");
        }



        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var routeData = httpContext.Request.RequestContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            var action = routeData.GetRequiredString("action");
            var isAuthorized = base.AuthorizeCore(httpContext);

            if (!isAuthorized)
            {
                return false;
            }


            string GrupoUsuario = string.Join("","User","Admin"); // Call another method to get rights of the user from DB



            if (GrupoUsuario.Contains(this.AccessLevel.Split(';').FirstOrDefault()))
            {
                return true;
            }
            else
            {
                return false;
            }





        }

    }
}