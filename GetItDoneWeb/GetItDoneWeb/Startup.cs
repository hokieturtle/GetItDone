using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GetItDoneWeb.Startup))]
namespace GetItDoneWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
