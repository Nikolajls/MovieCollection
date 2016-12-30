// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Collections.Generic;
using FoxTales.Infrastructure.DomainFramework;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.OperationFramework;

namespace FoxTales.Infrastructure.UnitOfWorkFramework.Interfaces
{

    public delegate void CommitSucceededHandler();
    public delegate void CommitFailedHandler(Exception ex);
    public delegate void CommittingHandler();

    public interface IUnitOfWork<TIdentity> where TIdentity : struct 
    {
        event CommitSucceededHandler CommitSucceeded;
        event CommitFailedHandler CommitFailed;
        event CommittingHandler Committing;

        void RegisterAdded(EntityBase<TIdentity> entity, IUnitOfWorkRepository<TIdentity> repository);
        void RegisterChanged(EntityBase<TIdentity> entity, IUnitOfWorkRepository<TIdentity> repository);
        void RegisterRemoved(EntityBase<TIdentity> entity, IUnitOfWorkRepository<TIdentity> repository);
        OperationResult<UnitOfWorkErrorCode> Commit();

        IEnumerable<T> GetAddedQueue<T>() where T : EntityBase<TIdentity>;
    }

    public interface IUnitOfWork : IUnitOfWork<Guid>
    { }

    public enum UnitOfWorkErrorCode
    {
        InvalidEntity,
        Unknown
    }
}
