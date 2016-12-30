using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EasyNetQ;
using FoxTales.Infrastructure.DependencyInjection;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public static class AutofacBootstrapperExtensions
    {
        public static AutofacBootstrapper ConfigureServiceBus(this AutofacBootstrapper me)
        {
            me.Builder.RegisterType<ValhalBus>().As<IValhalBus>().SingleInstance();
            me.Builder.RegisterType<BusHelper>().As<IBusHelper>().SingleInstance();
            me.Builder.Register(c => BusInitializer.CreateBus()).As<IBus>().SingleInstance();
            return me;
        }
    }
}
