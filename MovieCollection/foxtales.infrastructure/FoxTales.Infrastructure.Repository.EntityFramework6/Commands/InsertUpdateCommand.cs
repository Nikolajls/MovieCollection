using System;
using System.Data.Entity;
using System.Linq.Expressions;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.DTOFramework;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Commands
{
    public abstract class InsertUpdateCommand<T, TIdentity, TDTO, TDTOMapper> : CommandBase where T : ObjectBase<TIdentity>, new() where TIdentity : struct where TDTO : class, ILinkedDTO<TIdentity>, new() where TDTOMapper : IDTOMapper<T, TDTO>, new()
    {
        protected readonly TDTO DTO;

        protected InsertUpdateCommand(TDTO dto) 
        {
            DTO = dto;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            context.Configuration.ValidateOnSaveEnabled = false;

            if (!DTO.Id.Equals(default(TIdentity)))
            {
                var cmd = new UpdateCommand<T, TIdentity, TDTO, TDTOMapper>(DTO);
                StartChildCommand(cmd);
            }
            else
            {
                var cmd = new InsertCommand<T, TIdentity, TDTO, TDTOMapper>(DTO);
                StartChildCommand(cmd);
            }
        }

        protected abstract Expression<Func<T, bool>> GetIdExpression();
    }
    
    public class InsertUpdateCommand<T, TDTO, TDTOMapper> : InsertUpdateCommand<T, Guid, TDTO, TDTOMapper> where T : ObjectBase<Guid>, new() where TDTO : class, ILinkedDTO<Guid>, new() where TDTOMapper : IDTOMapper<T, TDTO>, new()
    {
        public InsertUpdateCommand(TDTO dto)
            : base(dto)
        {
        }

        protected override Expression<Func<T, bool>> GetIdExpression()
        {
            return t => t.Id == DTO.Id;
        }

    }

    public class InsertUpdateCommand<T, TDTO> : InsertUpdateCommand<T, TDTO, AutoDTOMapper<T, Guid, TDTO>> where TDTO : class, ILinkedDTO<Guid>, new() where T : ObjectBase<Guid>, new()
    {
        public InsertUpdateCommand(TDTO dto) : base(dto)
        {
        }
    }

    public class InsertUpdateCommandInt<T, TDTO, TDTOMapper> : InsertUpdateCommand<T, int, TDTO, TDTOMapper> where T : ObjectBase<int>, new() where TDTO : class, ILinkedDTO<int>, new() where TDTOMapper : IDTOMapper<T, TDTO>, new()
    {
        public InsertUpdateCommandInt(TDTO dto)
            : base(dto)
        {
        }

        protected override Expression<Func<T, bool>> GetIdExpression()
        {
            return t => t.Id == DTO.Id;
        }
    }

    public class InsertUpdateCommandInt<T, TDTO> : InsertUpdateCommandInt<T, TDTO, AutoDTOMapper<T, int, TDTO>> where TDTO : class, ILinkedDTO<int>, new() where T : ObjectBase<int>, new()
    {
        public InsertUpdateCommandInt(TDTO dto) : base(dto)
        {
        }
    }
    
}
