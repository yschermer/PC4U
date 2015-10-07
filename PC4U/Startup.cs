using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PC4U.Startup))]
namespace PC4U
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
