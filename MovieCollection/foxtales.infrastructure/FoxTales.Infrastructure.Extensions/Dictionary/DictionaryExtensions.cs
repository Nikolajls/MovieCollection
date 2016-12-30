using System.Collections.Generic;

namespace FoxTales.Infrastructure.Extensions.Dictionary
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return @default;
        }
    }
}
