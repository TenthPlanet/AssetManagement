using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssetManagement.WebUI.Startup))]
namespace AssetManagement.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
