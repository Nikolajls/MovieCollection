// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using FoxTales.Infrastructure.DomainFramework.Generics;

namespace FoxTales.Infrastructure.UnitOfWorkFramework.Interfaces
{
    public interface IUnitOfWorkRepository<TIdentity> where TIdentity : struct 
    {
        void PersistNewItem(EntityBase<TIdentity> item);
        void PersistUpdatedItem(EntityBase<TIdentity> item);
        void PersistDeletedItem(EntityBase<TIdentity> item);
    }

    public interface IUnitOfWorkRepository : IUnitOfWorkRepository<Guid>
    { }
}
