// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.OperationFramework;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;

namespace FoxTales.Infrastructure.RepositoryFramework.Interfaces
{

    public interface IRepository<T, in TIdentity> : IReadRepository<T> where T : EntityBase<TIdentity> where TIdentity : struct
    {
        T FindById(TIdentity id);
        IQueryable<T> FindByIds(params TIdentity[] ids);
        OperationResult<RepositoryErrorCode> Add(T item);
        T this[TIdentity id] { get; set; }
        void Remove(T item);
        OperationResult<RepositoryErrorCode> SoftDelete(T item);
        OperationResult<RepositoryErrorCode> Restore(T item);
        OperationResult<RepositoryErrorCode> Update(T item);
        OperationResult<RepositoryErrorCode> Update(T item, params Expression<Func<T, object>>[] properties);
        OperationResult<RepositoryErrorCode> AddOrUpdate(T item);
        IQueryable<T> QueryNew(ISpecification<T> specification = null,SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude);
    }

    public interface IRepository<T> : IRepository<T, Guid> where T : EntityBase<Guid>
    { }

    public enum RepositoryErrorCode
    {
        PersistenceSpecificationNotSatisfied,
        NoUnitOfWork,
        UpdateEntityNotFound
    }
}
