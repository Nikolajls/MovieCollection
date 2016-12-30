// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Linq.Expressions;

namespace FoxTales.Infrastructure.SpecificationFramework.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> IsSatisfied();

        bool IsSatisfied(T item);

        ISpecification<T> And(ISpecification<T> other);

        ISpecification<T> Or(ISpecification<T> other);

        ISpecification<T> Not();
    }
}
