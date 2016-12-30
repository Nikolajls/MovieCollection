// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;

namespace FoxTales.Infrastructure.SpecificationFramework
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> IsSatisfied();
        public bool IsSatisfied(T item)
        {
            return IsSatisfied().Compile().Invoke(item);
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        protected static class Utility
        {
            private static Expression<Func<T, bool>> Compose(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second, Func<Expression, Expression, Expression> merge)
            {
                // build parameter map (from parameters of second to parameters of first)
                var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

                // replace parameters in the second lambda expression with parameters from the first
                var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

                // apply composition of lambda expression bodies to parameters from the first expression 
                return Expression.Lambda<Func<T, bool>>(merge(first.Body, secondBody), first.Parameters);
            }

            public static Expression<Func<T, bool>> And(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            {
                return Compose(first, second, Expression.And);
            }

            public static Expression<Func<T, bool>> Or(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            {
                return Compose(first, second, Expression.Or);
            }

            private class ParameterRebinder : ExpressionVisitor
            {

                private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

                private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
                {
                    _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
                }

                public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
                {
                    return new ParameterRebinder(map).Visit(exp);
                }

                protected override Expression VisitParameter(ParameterExpression p)
                {

                    ParameterExpression replacement;
                    if (_map.TryGetValue(p, out replacement))
                    {
                        p = replacement;
                    }
                    return base.VisitParameter(p);
                }
            }
        }
    }
}
