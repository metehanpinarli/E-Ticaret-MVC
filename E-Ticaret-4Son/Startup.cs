using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Ticaret_4Son.Startup))]
namespace E_Ticaret_4Son
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
