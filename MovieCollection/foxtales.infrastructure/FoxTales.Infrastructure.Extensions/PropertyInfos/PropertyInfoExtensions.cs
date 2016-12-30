using System;
using System.Reflection;

namespace FoxTales.Infrastructure.Extensions.PropertyInfos
{
    public static class PropertyInfoExtensions
    {
        public static TReturn GetAttribute<TAttribute, TReturn>(this PropertyInfo propertyInfo, Func<TAttribute, TReturn> func, TReturn def = default(TReturn)) where TAttribute : Attribute
        {
            var attribute = propertyInfo.GetCustomAttribute<TAttribute>(true);
            return attribute == null ? def : func(attribute);
        }
    }
}
