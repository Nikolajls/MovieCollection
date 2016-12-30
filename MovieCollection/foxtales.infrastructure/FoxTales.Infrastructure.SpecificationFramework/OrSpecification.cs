// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Linq.Expressions;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;

namespace FoxTales.Infrastructure.SpecificationFramework
{
    public class OrSpecification<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }
        public override Expression<Func<T, bool>> IsSatisfied()
        {
            return Utility.Or(_left.IsSatisfied(), _right.IsSatisfied());
        }
    }
}
