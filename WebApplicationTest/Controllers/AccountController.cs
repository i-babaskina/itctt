using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplicationTest.Models;
using WebApplicationTest.Helpers;
using WebApplicationTest.Providers;
using System.Web.Security;

namespace WebApplicationTest.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private CustomMembershipProvider provider = new CustomMembershipProvider();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(String returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignIn(String returnUrl)
        {

            Boolean b = Request.IsAuthenticated;
            string input = Converters.ConvertInputStreamToString(Request.InputStream);
            User user = Converters.ConvertLoginInputToUser(input);
            var appUser = new ApplicationUser { UserName = user.Login};
            var result = UserManager.Create(appUser, user.Password);

            if (provider.ValidateUser(user.Login, user.Password))
            {
                FormsAuthentication.SetAuthCookie(appUser.UserName, true);
                RedirectToAction("Index", "Goods");
                return Json(new { success = true, returnUrl = returnUrl });
            }
            else return Json(new { success = false, message = "Invalid login or password,"});
        }

        [AllowAnonymous]
        public void AddUser()
        {
            var user = provider.CreateUser("testuser", "123456");
            var name = user.UserName;
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}