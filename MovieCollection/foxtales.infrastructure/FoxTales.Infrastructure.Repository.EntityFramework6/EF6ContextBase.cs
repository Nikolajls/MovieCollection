// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
#if DEBUG
using System.Diagnostics;
#endif
using System.Reflection;
using System.Text;
using FoxTales.Infrastructure.DTOFramework;
using JetBrains.Annotations;

namespace FoxTales.Infrastructure.Repository.EntityFramework6
{
    public abstract class EF6ContextBase : DbContext
    {
        [CanBeNull]
        public Assembly ConfigurationAssembly { get; set; }

        [Obsolete("Should not be used as migration won't work using this overload.")]
        protected EF6ContextBase()
        { }

        protected EF6ContextBase(Assembly configurationAssembly)
        {
            ConfigurationAssembly = configurationAssembly;
#if DEBUG
            Database.Log = s =>
            {
                Trace.WriteLine(s.Trim(), "SQL");
            };
#endif
        }

        protected EF6ContextBase(Assembly configurationAssembly, string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            ConfigurationAssembly = configurationAssembly;
#if DEBUG
            Database.Log = s =>
            {
                Trace.WriteLine(s.Trim(), "SQL");
            };
#endif
        }

        protected EF6ContextBase(Assembly configurationAssembly, DbConnection existingConnection)
            : base(existingConnection, true)
        {
            ConfigurationAssembly = configurationAssembly;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(ConfigurationAssembly);
        }
    }
}
