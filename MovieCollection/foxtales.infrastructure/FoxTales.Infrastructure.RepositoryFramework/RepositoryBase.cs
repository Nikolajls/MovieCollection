// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.OperationFramework;
using FoxTales.Infrastructure.RepositoryFramework.Interfaces;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;
using FoxTales.Infrastructure.UnitOfWorkFramework.Interfaces;
using JetBrains.Annotations;

namespace FoxTales.Infrastructure.RepositoryFramework
{
    public abstract class RepositoryBase<T, TIdentity> : IRepository<T, TIdentity>, IUnitOfWorkRepository<TIdentity>
        where T : EntityBase<TIdentity>
        where TIdentity : struct
    {
        private readonly IUnitOfWork<TIdentity> _unitOfWork;

        [CanBeNull]
        private readonly IPersistenceSpecification<T> _persistenceSpecification;

        protected IUnitOfWork<TIdentity> UnitOfWork
        {
            get { return _unitOfWork; }
        }

        protected RepositoryBase([NotNull] IUnitOfWork<TIdentity> unitOfWork, [CanBeNull] IPersistenceSpecification<T> persistenceSpecification = null)
        {
            _unitOfWork = unitOfWork;
            _persistenceSpecification = persistenceSpecification;
        }

        public abstract T FindById(TIdentity id);
        public abstract IQueryable<T> FindByIds(params TIdentity[] ids);

        public OperationResult<RepositoryErrorCode> Add(T item)
        {
            var result = new OperationResult<RepositoryErrorCode>();
            if (_persistenceSpecification != null && !_persistenceSpecification.IsSatisfied(item)) result.AddError(RepositoryErrorCode.PersistenceSpecificationNotSatisfied);

            if (_unitOfWork == null) result.AddError(RepositoryErrorCode.NoUnitOfWork);
            else _unitOfWork.RegisterAdded(item, this);

            return result;
        }

        public T this[TIdentity id]
        {
            get { return FindById(id); }
            set
            {
                if (id.Equals(default(TIdentity)) || FindById(id) == null)
                {
                    Add(value);
                }
                else
                {
                    _unitOfWork.RegisterChanged(value, this);
                }
            }
        }

        public void Remove(T item)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.RegisterRemoved(item, this);
            }
        }

        public OperationResult<RepositoryErrorCode> SoftDelete(T item)
        {
            item.SoftDeletedDate = DateTime.Now;
            return Update(item);
        }

        public OperationResult<RepositoryErrorCode> Restore(T item)
        {
            item.SoftDeletedDate = null;
            return Update(item);
        }

        public OperationResult<RepositoryErrorCode> Update(T item)
        {
            var result = new OperationResult<RepositoryErrorCode>();
            if (_persistenceSpecification != null && !_persistenceSpecification.IsSatisfied(item)) result.AddError(RepositoryErrorCode.PersistenceSpecificationNotSatisfied);

            if (_unitOfWork == null) result.AddError(RepositoryErrorCode.NoUnitOfWork);
            else _unitOfWork.RegisterChanged(item, this);

            return result;
        }

        public abstract OperationResult<RepositoryErrorCode> Update(T item, params Expression<Func<T, object>>[] properties); 

        public OperationResult<RepositoryErrorCode> AddOrUpdate(T item)
        {
            if (FindById(item.Id) == null) return Add(item);

            return Update(item);
        }

        public abstract IQueryable<T> Query(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude);
        public abstract IQueryable<T> QueryNew(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude);
        public void PersistNewItem(EntityBase<TIdentity> item)
        {
            PersistNewItem((T)item);
        }

        public void PersistUpdatedItem(EntityBase<TIdentity> item)
        {
            PersistUpdatedItem((T)item);
        }

        public void PersistDeletedItem(EntityBase<TIdentity> item)
        {
            PersistDeletedItem((T)item);
        }

        protected abstract void PersistNewItem(T item);
        protected abstract void PersistUpdatedItem(T item);
        protected abstract void PersistDeletedItem(T item);

    }

    public abstract class RepositoryBase<T> : RepositoryBase<T, Guid> where T : EntityBase<Guid>
    {
        protected RepositoryBase([NotNull] IUnitOfWork unitOfWork, [CanBeNull] IPersistenceSpecification<T> persistenceSpecification = null) : base(unitOfWork, persistenceSpecification)
        {
        }
    }
}