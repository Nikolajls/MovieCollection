// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using System.Data.Entity;
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;
using FoxTales.Infrastructure.DependencyInjection.Extensions;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Extensions
{
    public static class AutofacBootstrapperExtensions
    {
        public static AutofacBootstrapper ConfigureEntityFramework6<TContext, TConfigurationSample>(this AutofacBootstrapper me, IDatabaseInitializer<TContext> initializer = null) where TContext : DbContext, new()
        {
            me.Builder.RegisterType<TContext>().As<DbContext>().OnActivated(args =>
            {
                Database.SetInitializer(initializer);
                var ef6Context = args.Instance as EF6ContextBase;
                if (ef6Context != null)
                {
                    ef6Context.ConfigurationAssembly = typeof(TConfigurationSample).Assembly;
                }
            }).InstanceByApplicationType(me.ApplicationType);
            return me;
        }
    }
}
