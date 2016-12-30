using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.DomainFramework.Generics;
using FoxTales.Infrastructure.QueryFramework;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Queries
{
    public class FindAllQuery<T, TIdentity> : FindQueryBase<T, TIdentity> where T : ObjectBase<TIdentity> where TIdentity : struct
    {
        private readonly List<ISortDescription<T>> _orderByClauses;

        public FindAllQuery(params ISortDescription<T>[] orderByClauses)
        {
            _orderByClauses = orderByClauses.ToList();
        }

        public override Expression<Func<T, bool>> GetFilter()
        {
            return null;
        }

        public override IEnumerable<ISortDescription<T>> GetOrderByClauses()
        {
            return _orderByClauses;
        }
    }

    public class FindAllQuery<T> : FindAllQuery<T, Guid> where T : ObjectBase<Guid>
    {
        public FindAllQuery(params ISortDescription<T>[] orderByClauses) : base(orderByClauses)
        {
        }
    }   
}
