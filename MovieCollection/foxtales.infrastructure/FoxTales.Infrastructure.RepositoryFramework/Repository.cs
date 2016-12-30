// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.OperationFramework;
using FoxTales.Infrastructure.RepositoryFramework.Interfaces;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;

namespace FoxTales.Infrastructure.RepositoryFramework
{
    public class Repository<T, TIdentity> : IRepository<T, TIdentity> where T : EntityBase<TIdentity> where TIdentity : struct
    {
        private readonly RepositoryBase<T, TIdentity> _repository;
        private readonly IEnumerable<IDefaultQuerySpecification<T>> _defaultQuerySpecifications;

        public Repository(RepositoryBase<T, TIdentity> repository, IEnumerable<IDefaultQuerySpecification<T>> defaultQuerySpecifications)
        {
            _repository = repository;
            _defaultQuerySpecifications = defaultQuerySpecifications;
        }

        public IQueryable<T> Query(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            return _repository.Query(CombineSpecificationWithDefault(specification), softDeleteQuerySetting);
        }

        public T FindById(TIdentity id)
        {
            return _repository.FindById(id);
        }

        public IQueryable<T> FindByIds(params TIdentity[] ids)
        {
            return _repository.FindByIds(ids);
        }

        public OperationResult<RepositoryErrorCode> Add(T item)
        {
            return _repository.Add(item);
        }

        public T this[TIdentity id]
        {
            get { return _repository[id]; }
            set { _repository[id] = value; }
        }

        public void Remove(T item)
        {
            _repository.Remove(item);
        }

        public OperationResult<RepositoryErrorCode> SoftDelete(T item)
        {
            return _repository.SoftDelete(item);
        }

        public OperationResult<RepositoryErrorCode> Restore(T item)
        {
            return _repository.Restore(item);
        }

        public OperationResult<RepositoryErrorCode> Update(T item)
        {
            return _repository.Update(item);
        }

        public OperationResult<RepositoryErrorCode> Update(T item, params Expression<Func<T, object>>[] properties)
        {
            return _repository.Update(item, properties);
        }

        public OperationResult<RepositoryErrorCode> AddOrUpdate(T item)
        {
            return _repository.AddOrUpdate(item);
        }

        public IQueryable<T> QueryNew(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            return _repository.QueryNew(CombineSpecificationWithDefault(specification), softDeleteQuerySetting);
        }

        private ISpecification<T> CombineSpecificationWithDefault(ISpecification<T> specification)
        {
            if (specification == null && !_defaultQuerySpecifications.Any()) return null;

            var combinedSpecifications = specification ?? _defaultQuerySpecifications.First();
            return _defaultQuerySpecifications.Aggregate(combinedSpecifications, (current, defaultQuerySpecification) => current.And(defaultQuerySpecification));
        }
    }

    public class Repository<T> : Repository<T, Guid> where T : EntityBase<Guid>
    {
        public Repository(RepositoryBase<T, Guid> repository, IEnumerable<IDefaultQuerySpecification<T>> defaultQuerySpecifications) : base(repository, defaultQuerySpecifications)
        {
        }
    }
}