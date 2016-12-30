// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.OperationFramework;
using FoxTales.Infrastructure.RepositoryFramework;
using FoxTales.Infrastructure.RepositoryFramework.Interfaces;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;
using FoxTales.Infrastructure.UnitOfWorkFramework.Interfaces;
using JetBrains.Annotations;

namespace FoxTales.Infrastructure.Repository.Memory
{
    public class MemoryRepository<T, TIdentity> : RepositoryBase<T, TIdentity>
        where T : EntityBase<TIdentity>
        where TIdentity : struct
    {
        private readonly IDictionary<TIdentity, T> _entities = new Dictionary<TIdentity, T>();

        public MemoryRepository([NotNull] IUnitOfWork<TIdentity> unitOfWork, [CanBeNull] IPersistenceSpecification<T> persistenceSpecification = null)
            : base(unitOfWork, persistenceSpecification)
        {
        }

        public override T FindById(TIdentity id)
        {
            return _entities.ContainsKey(id) ? _entities[id] : null;
        }

        public override IQueryable<T> FindByIds(params TIdentity[] ids)
        {
            var result = (from id in ids where _entities.ContainsKey(id) select _entities[id]).ToList();
            return result.AsQueryable();
        }

        public override OperationResult<RepositoryErrorCode> Update(T item, params Expression<Func<T, object>>[] properties)
        {
            return Update(item);
        }

        public override IQueryable<T> Query(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            var result = _entities.Values.AsQueryable();
            if (specification != null) result = result.Where(specification.IsSatisfied());

            switch (softDeleteQuerySetting)
            {
                case SoftDeleteQuerySetting.Exclude:
                    result = result.Where(i => i.SoftDeletedDate == null);
                    break;
                case SoftDeleteQuerySetting.DeletedOnly:
                    result = result.Where(i => i.SoftDeletedDate != null);
                    break;
            }

            return result;
        }

        public override IQueryable<T> QueryNew(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            return new List<T>().AsQueryable();
        }

        protected override void PersistNewItem(T item)
        {
            _entities.Add(item.Id, item);
        }

        protected override void PersistUpdatedItem(T item)
        {
        }

        protected override void PersistDeletedItem(T item)
        {
            _entities.Remove(item.Id);
        }
    }

    public class MemoryRepositoryInt<T> : RepositoryBase<T, int> where T : EntityBase<int>
    {
        private readonly IDictionary<int, T> _entities = new Dictionary<int, T>();
        private readonly IDictionary<string, int> _identity = new Dictionary<string, int>();

        public MemoryRepositoryInt([NotNull] IUnitOfWork<int> unitOfWork, [CanBeNull] IPersistenceSpecification<T> persistenceSpecification = null)
            : base(unitOfWork, persistenceSpecification)
        {
        }

        public override T FindById(int id)
        {
            return _entities.ContainsKey(id) ? _entities[id] : null;
        }

        public override IQueryable<T> FindByIds(params int[] ids)
        {
            var result = (from id in ids where _entities.ContainsKey(id) select _entities[id]).ToList();
            return result.AsQueryable();
        }

        public override OperationResult<RepositoryErrorCode> Update(T item, params Expression<Func<T, object>>[] properties)
        {
            return Update(item);
        }

        public override IQueryable<T> Query(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            var result = _entities.Values.AsQueryable();
            if (specification != null) result = result.Where(specification.IsSatisfied());

            switch (softDeleteQuerySetting)
            {
                case SoftDeleteQuerySetting.Exclude:
                    result = result.Where(i => i.SoftDeletedDate == null);
                    break;
                case SoftDeleteQuerySetting.DeletedOnly:
                    result = result.Where(i => i.SoftDeletedDate != null);
                    break;
            }

            return result;
        }

        public override IQueryable<T> QueryNew(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            return new List<T>().AsQueryable();
        }

        protected override void PersistNewItem(T item)
        {
            if (item.Id == 0)
            {
                var latestEntity = Query().OrderByDescending(d => d.Id).FirstOrDefault();
                item.Id = latestEntity != null ? latestEntity.Id : 0;
                item.Id = item.Id + 1;
                UpdateChildrenIdentities(item);
            }

            _entities.Add(item.Id, item);
        }

        private void UpdateChildrenIdentities(T item)
        {
            var sourceMembers = typeof(T).GetProperties();
            var children = sourceMembers.Where(d => typeof(EntityBase<int>).IsAssignableFrom(d.PropertyType) || typeof(EntityBase<int>).IsAssignableFrom(d.PropertyType.GetGenericArguments().FirstOrDefault()));
            foreach (var propertyInfo in children)
            {
                if (propertyInfo.GetValue(item) == null) continue;
                if (!_identity.ContainsKey(propertyInfo.GetType().Name)) _identity.Add(propertyInfo.GetType().Name, 0);
                if (propertyInfo.PropertyType.IsArray || typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) || typeof(IEnumerable<>).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var childrenList = propertyInfo.GetValue(item) as IEnumerable<EntityBase<int>>;
                    if (childrenList == null) continue;
                    foreach (var entity in childrenList)
                    {
                        if (entity.Id != 0)
                            continue;
                        if (!_identity.ContainsKey(entity.GetType().Name)) _identity.Add(entity.GetType().Name, 0);
                        var identity = _identity[entity.GetType().Name] + 1;
                        _identity[entity.GetType().Name] = identity;
                        entity.Id = identity;
                    }
                    propertyInfo.SetValue(item, childrenList);
                }
                else
                {
                    var identity = _identity[propertyInfo.GetType().Name] + 1;
                    _identity[propertyInfo.GetType().Name] = identity;
                    var childrenMember = propertyInfo.GetValue(item) as EntityBase<int>;
                    if (childrenMember == null || childrenMember.Id != 0) continue;
                    childrenMember.Id = identity;
                    propertyInfo.SetValue(item, childrenMember);
                }

            }
        }

        protected override void PersistUpdatedItem(T item)
        {
            UpdateChildrenIdentities(item);
        }

        protected override void PersistDeletedItem(T item)
        {
            _entities.Remove(item.Id);
        }
    }
}
