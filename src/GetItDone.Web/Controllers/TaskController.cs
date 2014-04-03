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
                return (from u in db.Users.Include("Tasks") where u.UserID == user.UserID select u.Tasks).FirstOrDefault<List<Task>>().Where(t => !t.Done).OrderBy(t => t.Priority).AsEnumerable<Task>();
            }
            return null;
        }

        // POST api/Task/{userid}
        [ResponseType(typeof(Task))]
        [HttpPost]
        public IHttpActionResult PostTask(Task task)
        {
            User user = CookieHelper.LoggedInUser(Request, db);
            if (user != null)
            {
                db.Entry(user).Collection(u => u.Tasks).Load();
                user.Tasks.Add(task);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            return StatusCode(HttpStatusCode.Forbidden);

        }

        // POST api/Task/{userid}
        [HttpPost]
        public IHttpActionResult PostSort(Task postedTask)
        {
            User user = CookieHelper.LoggedInUser(Request, db);
            if (user != null)
            {
                db.Entry(user).Collection(u => u.Tasks).Load();

                Task movedTask = user.Tasks.Find(t => t.TaskID == postedTask.TaskID);
                if (movedTask.Priority != postedTask.Priority)
                {
                    if (movedTask.Priority > postedTask.Priority)
                    {
                        //The task has become more of a priority
                        //Move all tasks between the old position and the new position down one
                        foreach (Task t in user.Tasks.Where(t => !t.Done && t.Priority < movedTask.Priority && t.Priority >= postedTask.Priority))
                        {
                            t.Priority++;
                        }
                    }
                    else
                    {
                        //The task's priority value has increased which means it became less of a priority
                        foreach (Task t in user.Tasks.Where(t => !t.Done && t.Priority > movedTask.Priority && t.Priority<= postedTask.Priority))
                        {
                            t.Priority--;
                        }
                    }
                    movedTask.Priority = postedTask.Priority;
                }


                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            return StatusCode(HttpStatusCode.Forbidden);

        }

        // Post api/Task/Done/{taskid}
        [HttpGet]
        public IHttpActionResult Done(int id)
        {
            User user = CookieHelper.LoggedInUser(Request, db);
            if (user != null)
            {
                db.Entry(user).Collection(u => u.Tasks).Load();
                Task task = user.Tasks.Where(t => t.TaskID == id).FirstOrDefault();
                if (task != null)
                {
                    task.Done = true;
                    db.SaveChanges();
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            return StatusCode(HttpStatusCode.BadRequest);
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