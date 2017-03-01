using Blog.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Filters
{
    public class ChangePasswordAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            if (actionName.ToLower() == "changepassword")
            {
                return;
            }

            var id = HttpContext.Current.User.Identity.GetUserId();
            if (id == null)
            {
                return;
            }

            var db = new ApplicationDbContext();
            var user = db.Users.Find(id);

            if (user.ChangePassword)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "ChangePassword" }
                    });
                base.OnActionExecuted(filterContext);
            }
        }
    }
}