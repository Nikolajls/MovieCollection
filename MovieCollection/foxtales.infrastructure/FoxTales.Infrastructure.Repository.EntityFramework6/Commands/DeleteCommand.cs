using System;
using System.Data.Entity;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.DTOFramework;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Commands
{
    public class DeleteCommand<T, TIdentity> : CommandBase where T : ObjectBase<TIdentity>, new() where TIdentity : struct
    {
        private readonly TIdentity _id;

        public DeleteCommand(TIdentity id) 
        {
            _id = id;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            var entity = new T { Id = _id };
            context.Set<T>().Attach(entity);
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
    }

    public class DeleteCommand<T> : DeleteCommand<T, Guid> where T : ObjectBase<Guid>, new()
    {
        public DeleteCommand(Guid id) : base(id)
        {
        }
    }
}
