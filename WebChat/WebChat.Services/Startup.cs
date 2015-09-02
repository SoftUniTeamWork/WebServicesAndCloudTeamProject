using Microsoft.Owin;
using Owin;
using WebChat.Services;

[assembly: OwinStartup(typeof(Startup))]
namespace WebChat.Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
