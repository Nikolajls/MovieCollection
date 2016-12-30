using System;
using System.Text.RegularExpressions;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class HoursField : AField
    {
        public override bool IsSatisfiedBy(DateTime date, string value)
        {
            return IsSatisfied(date.Hour.ToString(), value);
        }

        public override DateTime Increment(DateTime date)
        {
            var tDate = date.AddHours(-1);
            tDate = new DateTime(tDate.Year, tDate.Month, tDate.Day, tDate.Hour, 59, 59);
            return tDate;
        }

        public override bool Validate(string value)
        {
            return Regex.IsMatch(value, @"^[\*,\/\-0-9]+$");
        }
    }
}
