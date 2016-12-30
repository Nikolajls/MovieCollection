using System;
using System.Linq;
using System.Linq.Expressions;
using Autofac;

namespace FoxTales.Infrastructure.DTOFramework
{
    public class AutoDTOMapper<T, TIdentity, TDTO> : IDTOMapper<T, TDTO> where TDTO : IDTO, new() where TIdentity : struct
    {
        private static Expression<Func<T, TDTO>> _expression;

        public virtual Expression<Func<T, TDTO>> ToDTO(ILifetimeScope lifetimeScope)
        {
            if (_expression != null) return _expression;

            var sourceMembers = typeof(T).GetProperties();
            var destinationMembers = typeof(TDTO).GetProperties().Where(d => sourceMembers.Any(s => s.PropertyType == d.PropertyType && s.Name == d.Name));

            const string name = "src";
            var parameterExpression = Expression.Parameter(typeof(T), name);

            _expression = Expression.Lambda<Func<T, TDTO>>(
                Expression.MemberInit(
                    Expression.New(typeof(TDTO)),
                    // ReSharper disable CoVariantArrayConversion - fint nok, men dette er ikke til skrive operationer
                    destinationMembers.Select(dest => Expression.Bind(dest,
                        Expression.Property(
                            parameterExpression,
                            sourceMembers.First(pi => pi.Name == dest.Name)
                            )
                        )).ToArray()
                    // ReSharper restore CoVariantArrayConversion
                    ),
                parameterExpression
                );

            return _expression;
        }

        public virtual void UpdateDomainModel(ILifetimeScope lifetimeScope, T domainModel, TDTO dto, Action<string> propertyChanged = null)
        {
            foreach (var targetProperty in typeof(T).GetProperties())
            {
                if (targetProperty.Name == "Id" && targetProperty.PropertyType == typeof(TIdentity)) continue;
                var sourceProperty = typeof(TDTO).GetProperty(targetProperty.Name);
                if (sourceProperty == null) continue;
                if (sourceProperty.PropertyType != targetProperty.PropertyType) continue;

                var sourceValue = sourceProperty.GetValue(dto);
                targetProperty.SetValue(domainModel, sourceValue);
                if (propertyChanged != null)
                {
                    propertyChanged(targetProperty.Name);
                }
            }
        }
    }

    public class AutoDTOMapper<T, TDTO> : AutoDTOMapper<T, Guid, TDTO> where TDTO : IDTO, new()
    { }
    
}