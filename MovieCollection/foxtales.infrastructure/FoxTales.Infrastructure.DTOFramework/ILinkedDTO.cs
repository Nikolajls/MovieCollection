using System;

namespace FoxTales.Infrastructure.DTOFramework
{
    public interface ILinkedDTO<TIdentity> : IDTO where TIdentity : struct
    {
        TIdentity Id { get; set; }
    }

    public interface ILinkedDTO : ILinkedDTO<Guid>
    { }
}