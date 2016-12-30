// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;
using FoxTales.Infrastructure.DomainFramework.Generics;

namespace FoxTales.Infrastructure.DomainFramework
{
    public abstract class ObjectBase : ObjectBase<Guid>
    {
        protected ObjectBase()
        {
            Id = IdGenerator.NewId();
        }
    }
}
