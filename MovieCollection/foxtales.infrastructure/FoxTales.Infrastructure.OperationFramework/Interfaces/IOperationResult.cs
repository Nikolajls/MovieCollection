// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System.Collections.Generic;

namespace FoxTales.Infrastructure.OperationFramework.Interfaces
{
    public interface IOperationResult
    {
        IReadOnlyCollection<OperationLogBase> Errors { get; }
        IReadOnlyCollection<OperationLogBase> Warnings { get; } 
        bool Succeeded();
    }
}