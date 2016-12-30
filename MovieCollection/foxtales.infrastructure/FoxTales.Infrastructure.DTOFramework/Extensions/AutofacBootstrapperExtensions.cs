using Autofac;
using FluentValidation;
using FoxTales.Infrastructure.DependencyInjection;

namespace FoxTales.Infrastructure.DTOFramework.Extensions
{
    public static class AutofacBootstrapperExtensions
    {
        public static AutofacBootstrapper ConfigureDTOValidators<TDTOSample>(this AutofacBootstrapper me)
        {
            AssemblyScanner.FindValidatorsInAssemblyContaining<TDTOSample>().ForEach(x => me.Builder.RegisterType(x.ValidatorType).As(x.InterfaceType).SingleInstance());
            return me;
        }
    }
}