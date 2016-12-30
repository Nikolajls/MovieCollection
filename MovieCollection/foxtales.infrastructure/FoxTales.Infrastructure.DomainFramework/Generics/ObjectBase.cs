namespace FoxTales.Infrastructure.DomainFramework.Generics
{
    public abstract class ObjectBase<TIdentity> where TIdentity : struct 
    {
        public TIdentity Id { get; set; }

        public abstract void MarkModification();
    }
}
