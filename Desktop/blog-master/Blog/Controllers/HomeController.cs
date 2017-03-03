using Blog.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [RequireHttps]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold> ({1})</p><p>Message:</p><p>{2}</p>";
                    var from = "MyPortfolio<srschoonmaker@gmail.com>";
                    model.Body = "This is a message from your portfolio site. The name and the email of the contacting person is above.";

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"]) { Subject = "Portfolio Contact Email", Body = string.Format(body, model.FromName, model.FromEmail, model.Body), IsBodyHtml = true };

                    //var svc = new PersonalEmail(); await svc.SendAsync(email);

                    return RedirectToAction("Sent");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); await Task.FromResult(0); }
            }
            return View(model);
        }
        public ActionResult Index()
        {
            var listPosts = db.Posts.Include(p => p.CreatedBy).ToList();
            return View(listPosts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}