// Copyright (C) 2014 FoxTales
// Released under the MIT License

using System;

namespace FoxTales.Infrastructure.DomainFramework
{
    public abstract class EntityBase : ObjectBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }

        protected EntityBase()
        {
            CreatedDate = ModifiedDate = DateTime.Now;
        }

        public override bool Equals(object entity)
        {
            if (!(entity is EntityBase))
            {
                return false;
            }
            return (this == entity);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override void MarkModification()
        {
            ModifiedDate = DateTime.Now;
        }
    }
}
