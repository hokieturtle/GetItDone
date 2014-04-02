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
        public IQueryable<Task> GetTasks()
        {
            return db.Tasks;
        }

        public IEnumerable<Task> GetTask(int id)
        {
            User user = (from u in db.Users where u.UserID == id select u).FirstOrDefault<User>();
            return user.Tasks.Where(t => !t.Done).AsEnumerable<Task>();
        }

        // POST api/Task/{userid}
        [ResponseType(typeof(Task))]
        [HttpPost]
        public IHttpActionResult PostTask(int id, Task task)
        {
            User user = (from u in db.Users where u.UserID == id select u).FirstOrDefault<User>();
            task.Owner = user;

            db.Tasks.Add(task);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.TaskID }, task);
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