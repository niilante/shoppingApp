using Blog.Filters;
using System.Web;
using System.Web.Mvc;

namespace Blog
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ChangePasswordAttribute());
        }
    }
}
