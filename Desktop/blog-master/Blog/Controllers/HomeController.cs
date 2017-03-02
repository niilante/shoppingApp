using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //this.ControllerContext.HttpContext.User.Identity.Name;
           // ViewBag.Message = 
           //go to database and retrieve admin
           //database has 5 admins
           //how do you know which one to pick ?

            //admin1 created post1
            //admin2 created post2
            //post1 ? 
            return View();
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