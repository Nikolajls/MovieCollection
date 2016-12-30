using Autofac;
using FluentValidation;
using FoxTales.Infrastructure.DependencyInjection;
using System.Reflection;

namespace FoxTales.Infrastructure.CommandFramework.Extensions
{
    public static class AutofacBootstrapperExtensions
    {
        public static AutofacBootstrapper ConfigureCommandValidators<TCommandSample>(this AutofacBootstrapper me)
        {
            me.Builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TCommandSample))).As<IValidator>();
            return me;
        }
    } 
}