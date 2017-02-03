using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingApp.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace ShoppingApp.Controllers
{
   
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        [Authorize]
        public ActionResult Index()
        {
            var items = db.Items.ToList();
            return View(items);
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            Item item = db.Items.Find(id);
            var cartItems = db.ShoppingCarts.SqlQuery($"SELECT * FROM ShoppingCarts WHERE ItemId={id} AND CustomerId='{user.Id}'").ToList();
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewModel.ItemDetailsViewModel viewModel = new ViewModel.ItemDetailsViewModel();
            viewModel.Item = item;
            viewModel.IsAdded = cartItems.Count > 0;

            return View(viewModel);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Price,MediaUrl,Description,Created,Updated")] Item item)
        {
            if (ModelState.IsValid)
            {
                //ImageUploadValidator validator = new ImageUploadValidator();
                //if (validator.IsWebFriendlyImage(Image))
                //{
                //    var fileName = Path.GetFileName(Image.FileName);
                //    Image.SaveAs(Path.Combine(Server.MapPath("~/images/uploads/"), fileName));
                //    item.MediaUrl = "~/images/uploads/" + fileName;
                //}

                if (Request.Files.Count == 1)
                {
                    var image = Request.Files[0];

                    if (image != null && image.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + ".jpg";
                        var path = Path.Combine(Server.MapPath("~/images/uploads"), fileName);
                        image.SaveAs(path);
                        item.MediaUrl = $"~/images/uploads/{fileName}";
                    }
                }

                item.Created = System.DateTime.Now;
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,Created,Updated")] Item item)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count == 1)
                {
                    var image = Request.Files[0];

                    if (image != null && image.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + ".jpg";
                        var path = Path.Combine(Server.MapPath("~/images/uploads"), fileName);
                        image.SaveAs(path);
                        item.MediaUrl = $"~/images/uploads/{fileName}";
                    }
                }
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
