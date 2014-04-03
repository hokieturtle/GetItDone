using GetItDone.DAL;
using GetItDone.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetItDone.Web.Controllers
{
    public class HomeController : Controller
    {

        // GET: /Home/
        public ActionResult Index()
        {

            //Check that the are logged in, if they are not redirect to login
            User user = CookieHelper.LoggedInUser(Request);
            if(user != null)
            {
                return View(user);
            }
            
            return RedirectToAction("Login", "Auth");
        }
    }
}