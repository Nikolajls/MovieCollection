// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;
using FoxTales.Infrastructure.DependencyInjection.Extensions;
using FoxTales.Infrastructure.ViewFramework.Interfaces;

namespace FoxTales.Infrastructure.ViewFramework.Extensions
{
    public static class AutofacBootstrapperExtensions
    {
        public static AutofacBootstrapper ConfigureViewModelConverters<TViewModelConverterSample>(this AutofacBootstrapper me)
        {
            var viewModelAssembly = typeof(TViewModelConverterSample).Assembly;
            me.Builder.RegisterAssemblyTypes(viewModelAssembly).Where(t => t.GetInterface(typeof(IViewModel).FullName) != null).AsSelf();
            me.Builder.RegisterAssemblyTypes(viewModelAssembly).AsClosedTypesOf(typeof(ModelConverterBase<,>)).AsSelf().InstanceByApplicationType(me.ApplicationType);
            return me;
        }
    }
}
