using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmpresaDeViajes.Startup))]
namespace EmpresaDeViajes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
