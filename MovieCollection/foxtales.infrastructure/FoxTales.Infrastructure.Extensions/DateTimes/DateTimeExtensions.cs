using System;
using System.Diagnostics.Contracts;

namespace FoxTales.Infrastructure.Extensions.DateTimes
{
    public static class DateTimeExtensions
    {
        [Pure]
        public static DateTime Next(this DateTime date, TimeSpan timeSpan)
        {
            return new DateTime(((date.Ticks + timeSpan.Ticks - 1) / timeSpan.Ticks) * timeSpan.Ticks);
        }
    }
}
