using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dev_PC4U.Startup))]
namespace Dev_PC4U
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
