using System;
using System.Linq;
using System.Linq.Expressions;

namespace FoxTales.Infrastructure.QueryFramework
{
    public interface ISortDescription<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> query);
        IOrderedQueryable<T> Apply(IOrderedQueryable<T> query);
    }

    public class SortDescription<T, TKey> : ISortDescription<T>
    {
        public Expression<Func<T, TKey>> Expression { get; private set; }
        public SortDirection Direction { get; private set; }

        internal SortDescription(Expression<Func<T, TKey>> expression, SortDirection direction = SortDirection.Ascending)
        {
            Expression = expression;
            Direction = direction;
        }

        public IOrderedQueryable<T> Apply(IQueryable<T> query)
        {
            switch (Direction)
            {
                case SortDirection.Ascending:
                    return query.OrderBy(Expression);
                case SortDirection.Descending:
                    return query.OrderByDescending(Expression);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IOrderedQueryable<T> Apply(IOrderedQueryable<T> query)
        {
            switch (Direction)
            {
                case SortDirection.Ascending:
                    return query.ThenBy(Expression);
                case SortDirection.Descending:
                    return query.ThenByDescending(Expression);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static class SortDescription<T>
    {
        public static ISortDescription<T> Create<TKey>(Expression<Func<T, TKey>> expression, SortDirection direction = SortDirection.Ascending)
        {
            return new SortDescription<T,TKey>(expression, direction);
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
