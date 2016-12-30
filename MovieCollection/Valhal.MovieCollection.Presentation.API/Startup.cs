using System;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using System.Web.Http;
using System.Web.UI;
using Autofac;
using Autofac.Integration.WebApi;
using FoxTales.Infrastructure.DependencyInjection;
using FoxTales.Infrastructure.Repository.EntityFramework6.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Valhal.MovieCollection.Infrastructure.EF;
using Valhal.MovieCollection.Infrastructure.EF.Configurations.Movies;

[assembly: OwinStartup(typeof(Valhal.MovieCollection.Presentation.API.Startup))]

namespace Valhal.MovieCollection.Presentation.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder customApp)
        {
            //CORS
            customApp.UseCors(CorsOptions.AllowAll);

            //Autofac
            var config = new HttpConfiguration();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(GetConfiguration());

            //Routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Movie", action = "Get", id = RouteParameter.Optional }
                );
         

          
            //JSON output
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            customApp.UseWebApi(config);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }

        private IContainer GetConfiguration()
        {
            var bootstrapper = Settings.Settings.Instance.GetAutofacBootstrapper(ApplicationType.Desktop)
                .ConfigureEntityFramework6<MovieCollectionContext, MovieConfiguration>();


            var bootstrapperSetup = bootstrapper.Setup();
            return bootstrapperSetup;
        }
    }
}
