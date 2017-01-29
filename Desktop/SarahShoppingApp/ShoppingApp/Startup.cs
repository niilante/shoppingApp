using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShoppingApp.Startup))]
namespace ShoppingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
