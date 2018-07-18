using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          
            /*
             routes.MapRoute(
                name: "Account",
                url: "{controller}/{action}/{Accountid}",
                defaults: new { controller = "Account", action = "ReadAccount", 
                    Accountid = UrlParameter.Optional 
                }
            );
            */

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{cod_cliente}/{garantiaId}",
                defaults: new { controller = "Crm", action = "index", id = UrlParameter.Optional, cod_cliente = UrlParameter.Optional, garantiaId = UrlParameter.Optional }
            );


            
        }
    }
}