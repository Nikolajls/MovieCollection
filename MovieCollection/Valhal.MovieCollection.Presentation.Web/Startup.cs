using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Valhal.MovieCollection.Presentation.Web.Startup))]
namespace Valhal.MovieCollection.Presentation.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
