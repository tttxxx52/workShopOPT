using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkShop.Startup))]
namespace WorkShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
