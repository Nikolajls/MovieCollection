// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Linq;
using System.Linq.Expressions;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;

namespace FoxTales.Infrastructure.SpecificationFramework
{
    public class NotSpecification<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> _other;

        public NotSpecification(ISpecification<T> other)
        {
            _other = other;
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            Expression<Func<T, bool>> originalTree = _other.IsSatisfied();

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(originalTree.Body),
                originalTree.Parameters.Single()
            );
        }
    }
}