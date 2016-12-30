using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Autofac;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.QueryFramework;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Queries
{
    public abstract class FindByIdQuery<T, TIdentity> : QueryBase<T> where T : ObjectBase<TIdentity> where TIdentity : struct
    {
        protected override IOrderedQueryable<T> OnExecuting(ILifetimeScope lifetimeScope)
        {
            var context = lifetimeScope.Resolve<DbContext>();
            return context.Set<T>().Where(GetExpression()).OrderBy(t => t.Id);
        }

        protected abstract Expression<Func<T, bool>> GetExpression();
    }

    public class FindByIdQuery<T> : FindByIdQuery<T, Guid> where T : ObjectBase<Guid>
    {
        private readonly Guid[] _ids;

        public FindByIdQuery(params Guid[] ids)
        {
            _ids = ids;
        }

        protected override Expression<Func<T, bool>> GetExpression()
        {
            if (_ids.Count() == 1)
            {
                var id = _ids.Single();
                return t => t.Id == id;
            }

            return t => _ids.Contains(t.Id);
        }
    }

    public class FindByIdQueryInt<T> : FindByIdQuery<T, int> where T : ObjectBase<int>
    {
        private readonly int[] _ids;

        public FindByIdQueryInt(params int[] ids)
        {
            _ids = ids;
        }

        protected override Expression<Func<T, bool>> GetExpression()
        {
            if (_ids.Count() == 1)
            {
                var id = _ids.Single();
                return t => t.Id == id;
            }
            return t => _ids.Contains(t.Id);
        }
    }
}
