// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;
using FoxTales.Infrastructure.DependencyInjection.Extensions;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;

namespace FoxTales.Infrastructure.SpecificationFramework.Extensions
{
    public static class AutofacBootstrapperExtensions
    {
        public static AutofacBootstrapper ConfigureSpecifications<TSpecificationSample>(this AutofacBootstrapper me)
        {
            var specificationAssembly = typeof(TSpecificationSample).Assembly;
            me.Builder.RegisterAssemblyTypes(specificationAssembly)
                .Where(
                    t =>
                        t.GetInterface(typeof (IDefaultQuerySpecification<>).FullName) != null ||
                        t.GetInterface(typeof (IPersistenceSpecification<>).FullName) != null)
                .AsImplementedInterfaces()
                .InstanceByApplicationType(me.ApplicationType);
            return me;
        }
    }
}
