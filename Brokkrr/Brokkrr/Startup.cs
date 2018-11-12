using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BrokerMVC.Startup))]
namespace BrokerMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
