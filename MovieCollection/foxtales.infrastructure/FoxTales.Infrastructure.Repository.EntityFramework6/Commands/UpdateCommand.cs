using System;
using System.Data.Entity;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.DTOFramework;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Commands
{
    public class UpdateCommand<T, TIdentity, TDTO, TMapper> : CommandBase where T : ObjectBase<TIdentity>, new() where TIdentity : struct where TDTO : ILinkedDTO<TIdentity>, new() where TMapper : IDTOMapper<T, TDTO>, new()

    {
        private readonly TDTO _dto;

        public UpdateCommand(TDTO dto) 
        {
            _dto = dto;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            context.Configuration.ValidateOnSaveEnabled = false;

            var mapper = new TMapper();
            var entity = new T();
            entity.Id = _dto.Id;
            context.Set<T>().Attach(entity);
            entity.MarkModification();
            mapper.UpdateDomainModel(lifetimeScope, entity, _dto, property => context.Entry(entity).Property(property).IsModified = true);
            context.SaveChanges();
        }
    }

    public class UpdateCommand<T, TDTO, TMapper> : UpdateCommand<T, Guid, TDTO, TMapper> where T : ObjectBase<Guid>, new() where TDTO : ILinkedDTO, new() where TMapper : IDTOMapper<T, TDTO>, new()
    {
        public UpdateCommand(TDTO dto) : base(dto)
        {
        }
    }
}