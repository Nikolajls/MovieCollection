// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
namespace FoxTales.Infrastructure.OperationFramework.Interfaces
{
    public interface IReturnOperationResult<out TResult> : IOperationResult
    {
        TResult Result { get; }
    }
}