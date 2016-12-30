// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using FoxTales.Infrastructure.OperationFramework;

namespace FoxTales.Infrastructure.FactoryFramework.Interfaces
{
    public interface IFactory<in TFactoryModel, TResult, TErrorCode>
        where TFactoryModel : class
        where TErrorCode : struct, IConvertible, IComparable, IFormattable
    {
        OperationResult<TErrorCode, TResult> Create(TFactoryModel model);
    }
}
