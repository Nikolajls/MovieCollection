using System;

namespace FoxTales.Infrastructure.DomainFramework.Generics
{
    public abstract class EntityBase<TIdentity> : ObjectBase<TIdentity> where TIdentity : struct
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
            if (!(entity is EntityBase<TIdentity>))
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
