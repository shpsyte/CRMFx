using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Services.Functions
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
         public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }

            var response = filterContext.HttpContext.Response;
            response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            response.Cache.SetValidUntilExpires(false);
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
        }
    }
}
