using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Test_PC4U.Startup))]
namespace Test_PC4U
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
