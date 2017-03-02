using Blog.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Blog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactMessage contact)
        {
            if (ModelState.IsValid)
            {
                var Emailer = new EmailService();
                var email = new IdentityMessage
                {
                    Subject = "Message",
                    Destination = ConfigurationManager.AppSettings["ContactEmail"],
                    Body = "You have a new message from: " + contact.Name + " (" +
                        contact.Email + ") with the following contents: \n\n" + contact.Message
                };
                Emailer.SendAsync(email);
                ViewBag.Message = "Your Message Was Sent Successfully.";
                return RedirectToAction("Index", "Posts");
            }
            return View(contact);
        }
           
    }
}