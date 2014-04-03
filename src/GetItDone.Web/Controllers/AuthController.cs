using GetItDone.DAL;
using GetItDone.DAL.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace GetItDone.Web.Controllers
{
    public class AuthController : Controller
    {

        private string LiveOauthUrlTemplate = "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=SCOPES&response_type=token&redirect_uri={1}";
        // GET: /Auth/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email, Password")] User User, FormCollection collection)
        {
            using (GetItDoneContext db = new GetItDoneContext())
            {
                 var authedUserQuery = from u in db.Users where u.Email.Equals(User.Email, StringComparison.InvariantCultureIgnoreCase) select u;

                 User authedUser = authedUserQuery.ToList()[0];
                 string hash = Crypto.HashPassword(User.Password);
                 if (Crypto.VerifyHashedPassword(authedUser.Password, User.Password))
                {
                    Guid sessionGuid = CreateSession(authedUser, db);

                    HttpCookie cookie = new HttpCookie("auth", sessionGuid.ToString());
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    Response.AppendCookie(cookie);
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.LoginMessage = "Invalid Password or Email";
                return View();
            }
        }

        // GET: /Auth/
        public ActionResult Login()
        {
            return View();
        }

        //private string ClientID
        //{
        //    get
        //    {
        //        return WebConfigurationManager.AppSettings["MSClientID"];
        //    }
        //}
        private Guid CreateSession(User User, GetItDoneContext db)
        {
            Guid guid = Guid.NewGuid();
            db.UserSessions.Add(new Session() { SessionUser = User, Created = DateTime.Now, Expires = DateTime.Now.AddMonths(1), ID = guid });
            db.SaveChanges();
            return guid;
        }

    }
}