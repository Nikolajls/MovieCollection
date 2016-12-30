using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace FoxTales.Infrastructure.MVCFramework.Extensions
{
    public static class EnumerableExtensions
    {
        [NotNull]
        public static IEnumerable<SelectListItem> ToSelectListItems<TEntry>([NotNull] this IEnumerable<TEntry> me, [CanBeNull] SelectListItem defaultItem, [NotNull] Func<TEntry, string> textSelector, [NotNull] Func<TEntry, string> valueSelector)
        {
            IList<SelectListItem> @default = new List<SelectListItem>();
            if (defaultItem != null)
            {
                @default.Add(defaultItem);
            }
            return @default.Concat(me.Select(entry => new SelectListItem { Text = textSelector(entry), Value = valueSelector(entry) }));
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this Enum enumValue)
        {
            return from Enum e in Enum.GetValues(enumValue.GetType())
                   select new SelectListItem
                   {
                       Selected = e.Equals(enumValue),
                       Text = e.ToDescription(),
                       Value = e.ToString()
                   };
        }

        public static string ToDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
