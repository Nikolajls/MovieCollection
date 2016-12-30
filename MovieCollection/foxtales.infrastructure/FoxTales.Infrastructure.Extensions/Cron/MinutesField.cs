using System;
using System.Text.RegularExpressions;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class MinutesField : AField
    {
        public override bool IsSatisfiedBy(DateTime date, string value)
        {
            return IsSatisfied(date.Minute.ToString(), value);
        }

        public override DateTime Increment(DateTime date)
        {
            var tDate = date.AddMinutes(-1);
            tDate = new DateTime(tDate.Year, tDate.Month, tDate.Day, tDate.Hour, tDate.Minute, 59);
            return tDate;
        }

        public override bool Validate(string value)
        {
            return Regex.IsMatch(value, @"^[\*,\/\-0-9]+$");
        }
    }
}
