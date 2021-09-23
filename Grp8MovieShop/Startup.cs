using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Grp8MovieShop.Startup))]
namespace Grp8MovieShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
