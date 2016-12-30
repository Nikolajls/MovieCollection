using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class MonthsField : AField
    {
        public override bool IsSatisfiedBy(DateTime date, string value)
        {
            List<string> days = new List<string>() { "JAN", "FEB", "MAR", "APR", "MAY", "JUN",
                "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            value = days.Aggregate(value, (current, day) => current.Replace(day, days.IndexOf(day).ToString()));
            return IsSatisfied(date.Month.ToString(), value);
        }

        public override DateTime Increment(DateTime date)
        {
            var tDate = date.AddMonths(-1);
            var daysInMonth = DateTime.DaysInMonth(tDate.Year, tDate.Month);
            date = new DateTime(tDate.Year, tDate.Month, daysInMonth, 23, 59, 59);

            return date;
        }

        public override bool Validate(string value)
        {
            return Regex.IsMatch(value, @"^[\*,\/\-0-9A-Z]+$");
        }
    }
}
