using Blog.Models;
using Blog.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    [RequireHttps]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View(db.Comments.OrderByDescending(p => p.CommentCreated).ToList());
        }

        // GET: Comments/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // POST: Comments/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include= "PostId,Body")] Comment comment)
        {
            var slug = db.Posts.Find(comment.PostId).Slug;
            if(ModelState.IsValid)
            {
                comment.AuthorId = User.Identity.GetUserId();
                comment.AuthorId = User.Identity.GetUserId();
                comment.CommentCreated = DateTimeOffset.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Posts", new { Slug = slug });
        }

        // GET: Comments/Edit/5
        [Authorize(Roles ="Admin, Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit([Bind(Include = "Id,PostId,Body")] Comment comment)
        {
            var slug = db.Posts.Find(comment.PostId).Slug;
            if (ModelState.IsValid)
            {
                db.Comments.Attach(comment);
                db.Entry(comment).Property(c => c.Body).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { Slug = slug });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            var slug = db.Posts.Find(comment.PostId).Slug;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Posts", new { Slug = slug });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
