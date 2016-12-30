using System;
using System.Text.RegularExpressions;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class YearsField : AField
    {
        public override bool IsSatisfiedBy(DateTime date, string value)
        {
            return IsSatisfied(date.Year.ToString(), value);
        }

        public override DateTime Increment(DateTime date)
        {
            var tDate = date.AddYears(-1);
            date = new DateTime(tDate.Year, 12, 31, 23, 59, 59);

            return date;
        }

        public override bool Validate(string value)
        {
            return Regex.IsMatch(value, @"^[\*,\/\-0-9]+$");
        }
    }
}