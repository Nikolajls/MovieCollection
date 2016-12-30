// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using Autofac;
using FoxTales.Infrastructure.DependencyInjection.Extensions;
using FoxTales.Infrastructure.UnitOfWorkFramework;
using FoxTales.Infrastructure.UnitOfWorkFramework.Interfaces;

namespace FoxTales.Infrastructure.DependencyInjection
{
    public class AutofacBootstrapper
    {
        private static ApplicationType _applicationType;
        private readonly ContainerBuilder _builder;

        public static IContainer Current { get; private set; }

        static AutofacBootstrapper()
        {
            Current = new ContainerBuilder().Build();
        }

        public ContainerBuilder Builder
        {
            get { return _builder; }
        }
        
        public ApplicationType ApplicationType
        {
            get { return _applicationType; }
        }

        public AutofacBootstrapper(ApplicationType type)
        {
            _applicationType = type;
            _builder = new ContainerBuilder();
            _builder.RegisterType<UnitOfWork<Guid>>().As<IUnitOfWork<Guid>>().InstanceByApplicationType(_applicationType);
            _builder.RegisterType<UnitOfWork<int>>().As<IUnitOfWork<int>>().InstanceByApplicationType(_applicationType);
        }

        public static ILifetimeScope BeginLifetimeScopeByApplicationType()
        {
            switch (_applicationType)
            {
                case ApplicationType.Desktop:
                    return Current.BeginLifetimeScope();
                case ApplicationType.WCF:
                case ApplicationType.MVC5:
                case ApplicationType.WebAPI:
                    return Current.BeginLifetimeScope("AutofacWebRequest");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public AutofacBootstrapper ConfigureRepositories<TRepositorySample>(string repositorySuffix = "Repository", string factorySuffix = "Factory") 
        {
            return ConfigureRepositories(typeof(TRepositorySample), repositorySuffix, factorySuffix);
        }

        public AutofacBootstrapper ConfigureRepositories(Type repositorySample, string repositorySuffix = "Repository", string factorySuffix = "Factory")
        {
            var repositoryAssembly = repositorySample.Assembly;
            _builder.RegisterAssemblyTypes(repositoryAssembly).Where(t => t.Name.EndsWith(repositorySuffix)).AsImplementedInterfaces().InstanceByApplicationType(_applicationType);
            _builder.RegisterAssemblyTypes(repositoryAssembly).Where(t => t.Name.EndsWith(factorySuffix)).AsImplementedInterfaces().InstanceByApplicationType(_applicationType);
            return this;
        }

        
        public AutofacBootstrapper ConfigureModelServices<TModelServiceSample>()
        {
            var modelAssembly = typeof (TModelServiceSample).Assembly;
            _builder.RegisterAssemblyTypes(modelAssembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstanceByApplicationType(_applicationType);
            return this;
        }

        public AutofacBootstrapper ConfigureApplicationServices<TApplicationServiceSample>()
        {
            var applicationAssembly = typeof (TApplicationServiceSample).Assembly;
            _builder.RegisterAssemblyTypes(applicationAssembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstanceByApplicationType(_applicationType);
            return this;
        }

        public IContainer Setup()
        {
            return Current = _builder.Build();
        }
    }

    public enum ApplicationType
    {
        Desktop,
        MVC5,
        WebAPI,
        WCF
    }
}
