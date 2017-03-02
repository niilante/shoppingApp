using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Blog.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Blog.Controllers
{
    [RequireHttps]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(int?page, string searchFilter, string searchTerm)
        {
            int pageSize = 5; //display five posts per page
            int pageNumber = (page ?? 1);
            var listPosts = db.Posts.AsQueryable();
            if (searchTerm != null)
            {
                page = 1;
            }
            else
            {
                searchTerm = searchFilter;
            }

            ViewBag.Search = searchTerm;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                listPosts = listPosts.Where(
                    p => p.Title.Contains(searchTerm) || 
                    p.Body.Contains(searchTerm) || 
                    p.Comments.Any(c => c.Body.Contains(searchTerm)));
            }
            return View(listPosts.OrderByDescending(p=>p.Created).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Admin()
        {
            return View(db.Posts.OrderByDescending(p => p.Created).ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.FirstOrDefault(p=>p.Slug == slug);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Title,Body,MediaUrl,Category")] Post post, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var Slug = StringUtilities.URLFriendly(post.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title.");
                    return View(post);
                }
                if (db.Posts.Any(p=>p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique.");
                    return View(post);
                }
                //restrict the the valid file formats to images only
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/MediaUrl/"), fileName));
                    post.MediaUrl = "~/Content/img/MediaUrl/" + fileName;
                }
                post.Slug = Slug;
                post.Created = DateTimeOffset.Now;
                post.Updated = DateTimeOffset.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,MediaUrl,Category,Slug")] Post post, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Attach(post);
                //restrict the the valid file formats to images only
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/MediaUrl/"), fileName));
                    post.MediaUrl = "~/Content/img/MediaUrl/" + fileName;
                    db.Entry(post).Property(p => p.MediaUrl).IsModified = true;
                }
                post.Updated = DateTimeOffset.Now;
                db.Entry(post).Property(p => p.Body).IsModified = true; //???
                db.Entry(post).Property(p => p.Category).IsModified = true; //???
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { slug = @post.Slug });
            }
            return View(post);

        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
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

//DONE--form with a URl.Action and input name=query
//create a viewbag in controller 
//ViewBag.Query = query;
//var qposts = db.Posts.AsQueryable();
//(page, query...)
//check if Search is not null
//if(!string.IsNullOrWhitespace(query))
    //qposts = qposts.Where(p => p.Title.Contains(query) || ... p.Comments.Any(c => c.Body.Contains(query) ...
      
    
//    return a list of posts where CONTAINS "string"
    