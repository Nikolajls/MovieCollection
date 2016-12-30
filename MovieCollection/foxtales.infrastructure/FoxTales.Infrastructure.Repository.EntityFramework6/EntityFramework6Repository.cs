// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.OperationFramework;
using FoxTales.Infrastructure.RepositoryFramework;
using FoxTales.Infrastructure.RepositoryFramework.Interfaces;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;
using FoxTales.Infrastructure.UnitOfWorkFramework.Interfaces;
using JetBrains.Annotations;

namespace FoxTales.Infrastructure.Repository.EntityFramework6
{
    public abstract class EntityFramework6Repository<T, TIdentity> : RepositoryBase<T, TIdentity> where T : EntityBase<TIdentity> where TIdentity : struct
    {
        private readonly DbContext _context;

        protected EntityFramework6Repository([NotNull] IUnitOfWork<TIdentity> unitOfWork, DbContext context, [CanBeNull] IPersistenceSpecification<T> persistenceSpecification = null)
            : base(unitOfWork, persistenceSpecification)
        {
            _context = context;
            unitOfWork.Committing += () => context.SaveChanges();
        }

        public override abstract T FindById(TIdentity id);

        public override IQueryable<T> FindByIds(params TIdentity[] ids)
        {
            return _context.Set<T>().Where(e => ids.Contains(e.Id));
        }

        public override OperationResult<RepositoryErrorCode> Update(T item, params Expression<Func<T, object>>[] properties)
        {
            var result = new OperationResult<RepositoryErrorCode>();

            var original = FindById(item.Id);
            if (original == null) result.AddError(RepositoryErrorCode.UpdateEntityNotFound);
            else
            {
                var entry = _context.Entry(item);
                if (entry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(item);
                    foreach (var property in properties)
                    {
                        entry.Property(property).IsModified = true;
                    }
                }
            }
            return result;
        }

        public override IQueryable<T> Query(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            var query = _context.Set<T>().AsQueryable();
            if (specification != null) query = query.Where(specification.IsSatisfied());
            switch (softDeleteQuerySetting)
            {
                case SoftDeleteQuerySetting.Exclude:
                    return query.Where(p => p.SoftDeletedDate.Equals(null));
                case SoftDeleteQuerySetting.DeletedOnly:
                    return query.Where(p => p.SoftDeletedDate.HasValue);
            }
            return query;
        }

        public override IQueryable<T> QueryNew(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude)
        {
            var query = UnitOfWork.GetAddedQueue<T>().AsQueryable();
            
            if (specification != null) query = query.Where(specification.IsSatisfied());
            switch (softDeleteQuerySetting)
            {
                case SoftDeleteQuerySetting.Exclude:
                    return query.Where(p => p.SoftDeletedDate.Equals(null));
                case SoftDeleteQuerySetting.DeletedOnly:
                    return query.Where(p => p.SoftDeletedDate.HasValue);
            }
            return query;
        }

        protected override void PersistNewItem(T item)
        {
            _context.Set<T>().Add(item);
        }

        protected override void PersistUpdatedItem(T item)
        {
        }

        protected override void PersistDeletedItem(T item)
        {
            _context.Set<T>().Remove(item);
        }
    }

    public class EntityFramework6RepositoryInt<T> : EntityFramework6Repository<T, int> where T : EntityBase<int>
// RepositoryBase<T, int> where T : EntityBase<int>
    {
        private readonly DbContext _context;

        public EntityFramework6RepositoryInt([NotNull] IUnitOfWork<int> unitOfWork, DbContext context, [CanBeNull] IPersistenceSpecification<T> persistenceSpecification = null) : base(unitOfWork, context, persistenceSpecification)
        {
            _context = context;
        }

        public override T FindById(int id)
        {            
            var local = QueryNew().SingleOrDefault(n => n.Id == id);
            return local ?? _context.Set<T>().SingleOrDefault(e => e.Id == id);
        }
    }


    public class EntityFramework6Repository<T> : EntityFramework6Repository<T, Guid> where T : EntityBase<Guid>
    {
        private readonly DbContext _context;

        public EntityFramework6Repository([NotNull] IUnitOfWork<Guid> unitOfWork, DbContext context, DbContext context1, [CanBeNull] IPersistenceSpecification<T> persistenceSpecification = null) : base(unitOfWork, context, persistenceSpecification)
        {
            _context = context1;
        }

        public override T FindById(Guid id)
        {            
            var local = QueryNew().SingleOrDefault(n => n.Id == id);
            return local ?? _context.Set<T>().SingleOrDefault(e => e.Id == id);
        }
    }
}
