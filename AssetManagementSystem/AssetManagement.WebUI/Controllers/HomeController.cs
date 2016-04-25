using AssetManagement.Domain.Abstract;
using AssetManagement.WebUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AssetManagement.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IAuthProvider authProvider;

        public HomeController(IAuthProvider auth)
        {
            authProvider = auth;
        }
        public ViewResult Index()
        {
            return View();
        }
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Asset"));
                }
                else
                {
                    TempData["Message"] = "Incorrect username or password";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Request.Cookies.Remove("Admin");
            FormsAuthentication.SignOut();
            //WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }
	}
}