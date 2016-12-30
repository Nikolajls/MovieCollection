namespace FoxTales.Infrastructure.DomainFramework.Generics
{
    public abstract class ValueBase<TIdentity> : ObjectBase<TIdentity> where TIdentity : struct
    {
        public override void MarkModification()
        { }
    }
}
