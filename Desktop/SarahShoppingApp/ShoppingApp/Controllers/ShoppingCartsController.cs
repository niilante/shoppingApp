using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingApp.Models;
using Microsoft.AspNet.Identity;

namespace ShoppingApp.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShoppingCarts
        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var shoppingCarts = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();
            if (shoppingCarts != null)
            {
                return View(shoppingCarts);
            }
            ViewBag.NoItem = "No item has been added";
            return View();
        }

        //GET: Shared/_CartItem 
        [Authorize]
        public PartialViewResult Cart()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null)
            {
                var shoppingCarts = db.ShoppingCarts.Where(s => s.CustomerId == user.Id);

                decimal shopTotal = 0;
                int shopCount = 0;

                foreach (var count in shoppingCarts)
                {
                    shopCount += count.Count;
                    shopTotal += db.Items.Where(t => t.Id == count.ItemId).Sum(t => t.Price) * count.Count;
                }
                ViewBag.itemCart = shopCount;
                ViewBag.itemTotal = shopTotal;

            }
            return PartialView("~/Views/Shared/_CartItem.cshtml");
        }

        // GET: ShoppingCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Itemid)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var exShopping = db.ShoppingCarts.Where(s => s.CustomerId == user.Id && s.ItemId == Itemid).ToList();
            if (exShopping.Count == 0)
            {
                ShoppingCart shoppingCart = new ShoppingCart();
                shoppingCart.CustomerId = user.Id;
                shoppingCart.ItemId = Itemid;
                shoppingCart.Item = db.Items.FirstOrDefault(i => i.Id == Itemid);
                shoppingCart.Count = 1;
                shoppingCart.Created = System.DateTime.Now;
                db.ShoppingCarts.Add(shoppingCart);
                db.SaveChanges();
                return RedirectToAction("Index", "Items");
            }

            foreach (var items in exShopping)
            {
                items.Count++;
                db.Entry(items).Property("Count").IsModified = true;
            };

            db.SaveChanges();

            return RedirectToAction("Index", "Items");
        }

        // GET: ShoppingCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", shoppingCart.ItemId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemId,Count,Created")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", shoppingCart.ItemId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            db.ShoppingCarts.Remove(shoppingCart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var shoppingCarts = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();
            if (shoppingCarts != null)
            {
                foreach (var del in shoppingCarts)
                {
                    db.ShoppingCarts.Remove(del);
                }

                db.SaveChanges();
            }
            return RedirectToAction("Index", "ShoppingCarts");
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
