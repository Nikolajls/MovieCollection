using System;
using System.Text.RegularExpressions;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class SecondsField : AField
    {
        public override bool IsSatisfiedBy(DateTime date, string value)
        {
            return IsSatisfied(date.Second.ToString(), value);
        }

        public override DateTime Increment(DateTime date)
        {
            return date.AddSeconds(-1);
        }

        public override bool Validate(string value)
        {
            return Regex.IsMatch(value, @"^[\*,\/\-0-9]+$");
        }
    }
}
