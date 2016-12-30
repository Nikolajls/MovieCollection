using System;
using System.Linq;
using System.Net;

namespace FoxTales.Infrastructure.Extensions.Objects
{
    public static class ObjectExtensions
    {
        public static TResult SafeGetter<T, TResult>(this T me, Func<T, TResult> getter, TResult def = default(TResult)) where T : class
        {
            return me == null ? def : getter(me);
        }

        public static IQueryable<T> ToQueryable<T>(this T me)
        {
            return new[] { me }.AsQueryable();
        }
        public static T HtmlEncodeDataObject<T>(this T message)
        {
            foreach (var property in message.GetType().GetProperties().Where(g => g.PropertyType == typeof(string)))
            {
                property.SetValue(message, WebUtility.HtmlEncode((string)property.GetValue(message, null)));
            }
            return message;
        }
    }
}
