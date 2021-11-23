using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArtSpace.Startup))]
namespace ArtSpace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
