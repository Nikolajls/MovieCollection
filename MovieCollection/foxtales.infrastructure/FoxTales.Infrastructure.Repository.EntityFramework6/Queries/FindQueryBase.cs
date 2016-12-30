using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Autofac;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.QueryFramework;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Queries
{
    public abstract class FindQueryBase<T, TIdentity> : QueryBase<T> where T : ObjectBase<TIdentity> where TIdentity : struct
    {
        public abstract Expression<Func<T, bool>> GetFilter();

        public abstract IEnumerable<ISortDescription<T>> GetOrderByClauses();

        protected override IOrderedQueryable<T> OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.LazyLoadingEnabled = false;

            IOrderedQueryable<T> orderedQueryable = null;
            var query = context.Set<T>().AsQueryable();
            var filter = GetFilter();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // ReSharper disable LoopCanBeConvertedToQuery - jeg kan ikke læse koden bagefter :P
            foreach (var orderByClause in GetOrderByClauses())
            // ReSharper restore LoopCanBeConvertedToQuery
            {
                orderedQueryable = orderedQueryable == null ? orderByClause.Apply(query) : orderByClause.Apply(orderedQueryable);
            }

            return orderedQueryable ?? query.OrderBy(e => e.Id);
        }
    }

    public abstract class FindQueryBase<T> : FindQueryBase<T, Guid> where T : ObjectBase<Guid>
    { }
}