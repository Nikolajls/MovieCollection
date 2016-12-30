using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class DayOfMonthField : AField
    {
        public override bool IsSatisfiedBy(DateTime date, string value)
        {
            if (value == "?")
            {
                return true;
            }

            var fieldValue = date.Day;

            if (value == "L")
            {
                return fieldValue == DateTime.DaysInMonth(date.Year, date.Month);
            }

            if (value.Contains('W'))
            {
                var targetDay = value.Substring(0, value.IndexOf('W'));
                return date.DayOfWeek == GetNearestWeekday(date.Year, date.Month, Convert.ToInt32(targetDay)).DayOfWeek;
            }

            return IsSatisfied(date.Day.ToString(), value);
        }

        public override DateTime Increment(DateTime date)
        {
            var tDate = date.AddDays(-1);
            tDate = new DateTime(tDate.Year, tDate.Month, tDate.Day, 23, 59, 59);
            return tDate;
        }

        public override bool Validate(string value)
        {
            return Regex.IsMatch(value, @"^[\*,\/\-\?LW0-9A-Za-z]+$");
        }

        private static DateTime GetNearestWeekday(int currentYear, int currentMonth, int targetDay)
        {
            var target = new DateTime(currentYear, currentMonth, targetDay);

            if ((Int32)target.DayOfWeek < 6)
            {
                return target;
            }

            var lastDayOfTheMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            foreach (var val in new []{-1, 1, -2, 2})
            {
                var adjusted = targetDay + val;
                if (adjusted > 0 && adjusted <= lastDayOfTheMonth)
                {
                    target = new DateTime(currentYear, currentMonth, adjusted);
                    if ((Int32) target.DayOfWeek < 6 && target.Month == currentMonth)
                    {
                        return target;
                    }
                }
            }
            return target;
        }
    }
}
