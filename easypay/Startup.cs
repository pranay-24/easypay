using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(easypay.Startup))]
namespace easypay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
