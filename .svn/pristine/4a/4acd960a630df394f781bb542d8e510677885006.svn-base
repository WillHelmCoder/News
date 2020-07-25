using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dal.Startup))]
namespace Dal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
