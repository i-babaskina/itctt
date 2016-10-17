using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplicationTest.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4netConfig.config", Watch = true)]
namespace WebApplicationTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
