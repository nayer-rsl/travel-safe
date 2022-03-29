using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PassionSem.Startup))]
namespace PassionSem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
