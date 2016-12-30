// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;
using FoxTales.Infrastructure.DependencyInjection.Extensions;
using FoxTales.Infrastructure.RepositoryFramework.Interfaces;

namespace FoxTales.Infrastructure.RepositoryFramework.Extensions
{
    public static class AutofacBootstrapperExtensions
    {
        public static AutofacBootstrapper ConfigureRepositoryFramework<TRepositoryImplementation>(this AutofacBootstrapper me)
        {
            return ConfigureRepositoryFramework(me, typeof (TRepositoryImplementation));
        }

        public static AutofacBootstrapper ConfigureRepositoryFramework(this AutofacBootstrapper me, Type repositoryImplementation)
        {
            me.Builder.RegisterGeneric(repositoryImplementation).As(typeof(IRepository<,>)).InstanceByApplicationType(me.ApplicationType);
            me.Builder.RegisterGeneric(repositoryImplementation).As(typeof(RepositoryBase<,>)).InstanceByApplicationType(me.ApplicationType);
            return me;
        }
    }
}
