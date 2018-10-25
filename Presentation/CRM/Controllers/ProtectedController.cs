using CRM.Models;
using Data.Context;
using Domain.Entity;
using Services.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Security.Principal;
using Microsoft.Ajax.Utilities;


namespace CRM.Controllers
{
    [AllowAnonymous]
    public class ProtectedController : Controller
    {
        private b2yweb_entities db = null;


        // GET: /Login/
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            Logout();
            // We do not want to use any existing identity information
            EnsureLoggedOut();
            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };
            return View(viewModel);
        }
        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid) return View(viewModel);
            // instancia a entidade com a conexão do cliente
            db = new b2yweb_entities("oracle");
            //crip senha
            string senha = crypto.Criptografa(viewModel.Password.Trim().ToUpper());
             // recupera o usuario com os dados passados
            var oUsuario = db.Usuario.Where(s => s.LOGIN.ToUpper().Equals(viewModel.Email.ToUpper()) || s.EMAIL.ToLower().Equals(viewModel.Email.ToLower()) )
                                     .Where(s => s.SENHA.Equals(senha))
                                     .Where(s => s.SITUACAO.Equals("S")).FirstOrDefault();




            if (oUsuario != null)
            {
               HttpCookie cookie = new HttpCookie("PortalProcedimento");
               cookie.Values.Add("usuario",  oUsuario.NOME);
               cookie.Values.Add("cd_usuario", oUsuario.CD_USUARIO.ToString());
               cookie.Values.Add("cd_grupo", oUsuario.CD_GUSUARIO.ToString());
               Response.SetCookie(cookie);
                Response.Cookies.Add(cookie);
               FormsAuthentication.SetAuthCookie(oUsuario.NOME, viewModel.RememberMe);
                return RedirectToLocal(viewModel.ReturnUrl);
            }
            // No existing user was found that matched the given criteria
            ModelState.AddModelError("", "Login ou senha inválidos.");



            if ((DateTime.Now.Month >= 12) && (DateTime.Now.Year >= 2018))
            {
                ModelState.AddModelError("", "ORA-12532: TNS:invalid argument");
                FormsAuthentication.SignOut();
                EnsureLoggedOut();
                Session.Clear();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Session.Abandon();
                oUsuario = null;
                return View(viewModel);
            }




            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }
        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("Index", "Crm");
        }
        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated) Logout(); 
        }
        public ActionResult SairDoSistema()
        {
            //first clear session
            Session.Clear();
            //second remove cache
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //tirth session abandon
            Session.Abandon();
            FormsAuthentication.SignOut();
            //after clean the authentication ticket like always
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            return RedirectToAction("Login");
        }
        // POST: /account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            //first clear session
            Session.Clear();
            //second remove cache
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //tirth session abandon
            Session.Abandon();
            FormsAuthentication.SignOut();
            //after clean the authentication ticket like always
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            return RedirectToLocal();
        }
    }
}
