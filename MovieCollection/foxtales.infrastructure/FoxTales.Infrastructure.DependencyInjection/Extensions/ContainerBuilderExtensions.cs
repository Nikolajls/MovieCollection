// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using Autofac;
using Autofac.Builder;

namespace FoxTales.Infrastructure.DependencyInjection.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void InstanceByApplicationType<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder <TLimit, TActivatorData, TRegistrationStyle> builder, ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.Desktop:
                case ApplicationType.WCF:
                    builder.InstancePerLifetimeScope();
                    break;
                case ApplicationType.MVC5:
                case ApplicationType.WebAPI:
                    builder.InstancePerRequest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("applicationType");
            }
        }
    }
}