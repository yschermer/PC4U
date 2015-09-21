using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Prod_PC4U.Startup))]
namespace Prod_PC4U
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
