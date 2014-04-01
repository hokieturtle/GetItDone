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
        //
        // GET: /Home/
        public ActionResult Index()
        {
            using(GetItDoneContext c = new GetItDoneContext())
            {
                var andrew = (from s in c.Users.Include("Tasks") where s.FirstName == "Andrew" select s).FirstOrDefault<User>();
                return View(andrew);
            }
        }
	}
}