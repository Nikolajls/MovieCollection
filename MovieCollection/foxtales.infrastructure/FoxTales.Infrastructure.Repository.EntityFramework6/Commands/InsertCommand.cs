using System;
using System.Data.Entity;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.DTOFramework;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Commands
{
    public class InsertCommand<T, TIdentity, TDTO, TMapper> : CommandBase where T : ObjectBase<TIdentity>, new() where TIdentity : struct where TMapper : IDTOMapper<T, TDTO>, new() where TDTO : class, ILinkedDTO<TIdentity>, new()
    {
        private readonly TDTO _dto;

        public InsertCommand(TDTO dto)
        {
            _dto = dto;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            var mapper = new TMapper();
            var entity = new T();

            mapper.UpdateDomainModel(lifetimeScope, entity, _dto);
            context.Set<T>().Add(entity);
            context.SaveChanges();
            _dto.Id = entity.Id;
        }
    }

    public class InsertCommand<T, TDTO, TMapper> : InsertCommand<T, Guid, TDTO, TMapper> where TMapper : IDTOMapper<T, TDTO>, new() where TDTO : class, ILinkedDTO, new() where T : ObjectBase<Guid>, new()
    {
        public InsertCommand(TDTO dto) : base(dto)
        {
        }
    }

    public class InsertCommand<T, TIdentity> : CommandBase where T : ObjectBase<TIdentity> where TIdentity : struct
    {
        private readonly T _entity;

        public InsertCommand(T entity) 
        {
            _entity = entity;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            context.Set<T>().Add(_entity);
            context.SaveChanges();
        }
    }

    public class InsertCommand<T> : InsertCommand<T, Guid> where T : ObjectBase<Guid>
    {
        public InsertCommand(T entity) : base(entity)
        {
        }
    }
}
