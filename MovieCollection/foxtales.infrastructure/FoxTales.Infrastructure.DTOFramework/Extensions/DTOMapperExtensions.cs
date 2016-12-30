using System.Linq;
using FoxTales.Infrastructure.DependencyInjection;

namespace FoxTales.Infrastructure.DTOFramework.Extensions
{
    public static class DTOMapperExtensions
    {
        public static TDTO ToDTO<T, TDTO>(this IDTOMapper<T, TDTO> mapper, T entity) where TDTO : IDTO, new()
        {
            var lifetimeScope = AutofacBootstrapper.BeginLifetimeScopeByApplicationType();
            return new[] { entity }.AsQueryable().Select(mapper.ToDTO(lifetimeScope)).SingleOrDefault();
        }
    }
}
