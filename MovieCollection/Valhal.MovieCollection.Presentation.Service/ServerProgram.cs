using System;
using System.Reflection;
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;
using FoxTales.Infrastructure.Repository.EntityFramework6.Extensions;
using Topshelf;
using Valhal.MovieCollection.Infrastructure.EF;
using Valhal.MovieCollection.Infrastructure.EF.Configurations.Movies;
using Valhal.MovieCollection.Infrastructure.Servicebus;

namespace Valhal.MovieCollection.Presentation.Service
{
    public class ServerProgram
    {
        public string ServiceDescription { get; set; } = "MovieCollection service for scheduling jobs and ServiceBus consuming";
        public string ServiceDisplayName { get; set; } = "MovieCollection";
        public string ServiceName { get; set; } = "MovieCollection";

        public void Run()
        {
            Console.WriteLine($"Starting Windows service:{ServiceName}");

            var container = InitializeAutofac();
            HostFactory.Run(x =>
            {
                x.Service<Servicen>(s =>
                {
                    s.ConstructUsing(() => container.Resolve<Servicen>());
                    s.WhenStarted(rs => { rs.Start(); });
                    s.WhenStopped(rs => { rs.Stop(); });

                });
                x.RunAsLocalSystem();
                x.SetDescription(ServiceDescription);
                x.SetDisplayName(ServiceDisplayName);
                x.SetServiceName(ServiceName);
                x.StartAutomatically();
            });
        }

        private IContainer InitializeAutofac()
        {
            var setup = new AutofacBootstrapper(ApplicationType.Desktop).
                ConfigureEntityFramework6<MovieCollectionContext, MovieConfiguration>().ConfigureServiceBus();
            setup.Builder.RegisterType<Servicen>().AsSelf();
            var currentAssembly = Assembly.GetAssembly(typeof (Servicen));

            setup.Builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => t.Name.EndsWith("Consumer"))
                .AsSelf()
                .InstancePerLifetimeScope();
            setup.Builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => t.Name.EndsWith("Responder"))
                .AsSelf()
                .InstancePerLifetimeScope();
            setup.Builder.RegisterType<Servicen>().AsSelf();

            return setup.Setup();
        }
    }
}
