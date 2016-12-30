// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System.Linq;
using FoxTales.Infrastructure.SpecificationFramework.Interfaces;

namespace FoxTales.Infrastructure.RepositoryFramework.Interfaces
{
    public interface IReadRepository<T>
    {
        IQueryable<T> Query(ISpecification<T> specification = null, SoftDeleteQuerySetting softDeleteQuerySetting = SoftDeleteQuerySetting.Exclude);
    }

    public enum SoftDeleteQuerySetting
    {
        Exclude,
        Include,
        DeletedOnly
    }
}