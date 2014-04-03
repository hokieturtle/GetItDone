using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GetItDone.DAL.Models;
using GetItDone.DAL;

namespace GetItDone.Web.Controllers
{
    public class TaskController : ApiController
    {
        private GetItDoneContext db = new GetItDoneContext();

        // GET api/Task
        public IEnumerable<Task> GetTask()
        {
            User user = CookieHelper.LoggedInUser(Request);
            if (user != null)
            {
                return (from u in db.Users.Include("Tasks") where u.UserID == user.UserID select u.Tasks).FirstOrDefault<List<Task>>().Where(t => !t.Done).AsEnumerable<Task>(); 
            }
            return null;
        }

        // POST api/Task/{userid}
        [ResponseType(typeof(Task))]
        [HttpPost]
        public IHttpActionResult PostTask(Task task)
        {
            User user = CookieHelper.LoggedInUser(Request, db);

            db.Entry(user).Collection(u => u.Tasks).Load();
            user.Tasks.Add(task);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // Post api/Task/Done/{taskid}
        [HttpGet]
        public IHttpActionResult Done(int id)
        {
            Task task = (from t in db.Tasks where t.TaskID == id select t).First<Task>();
            task.Done = true;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent); 
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.TaskID == id) > 0;
        }
    }
}