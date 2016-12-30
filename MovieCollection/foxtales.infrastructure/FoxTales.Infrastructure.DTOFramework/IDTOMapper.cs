using System;
using System.Linq.Expressions;
using Autofac;

namespace FoxTales.Infrastructure.DTOFramework
{
    public interface IDTOMapper<T, TDTO> where TDTO : IDTO, new()
    {
        Expression<Func<T, TDTO>> ToDTO(ILifetimeScope lifetimeScope);

        void UpdateDomainModel(ILifetimeScope lifetimeScope, T domainModel, TDTO dto, Action<string> propertyChanged = null);
    }
}
