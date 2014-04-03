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
        GetItDoneContext db = new GetItDoneContext();
        private string LiveOauthUrlTemplate = "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=SCOPES&response_type=token&redirect_uri={1}";
        // GET: /Auth/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email, Password")] User User, FormCollection collection)
        {

            var authedUser = (from u in db.Users where u.Email.Equals(User.Email, StringComparison.InvariantCultureIgnoreCase) select u).FirstOrDefault();
            //Check if we got any users
            if (authedUser!=null)
            {
                 //Check if the password is correct
                if (Crypto.VerifyHashedPassword(authedUser.Password, User.Password))
                {
                    Guid sessionGuid = CreateSession(authedUser, db);
                    //Set cookie for login
                    HttpCookie cookie = new HttpCookie("auth", sessionGuid.ToString());
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    Response.AppendCookie(cookie);
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.LoginMessage = "Invalid Password or Email";
            return View();

        }

        // GET: /Auth/
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            try
            {
                Guid AuthGuid = CookieHelper.AuthCookie(Request);
                var session = (from s in db.UserSessions where s.ID == AuthGuid select s).FirstOrDefault();
                if (session != null)
                {
                    db.UserSessions.Remove(session);
                    db.SaveChanges();
                }
            }
            catch (ArgumentException) { }
            return View("Login");
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