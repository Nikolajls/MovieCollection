using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class DayOfWeekField : AField
    {
        public override bool IsSatisfiedBy(DateTime date, string value)
        {
            if (value.Equals("?"))
            {
                return true;
            }

            value = ConvertLiterals(value);

            var currentYear = date.Year;
            var currentMonth = date.Month;
            var lastDayOfMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            if (value.Contains("L"))
            {
                var weekday = value.Substring(0, value.IndexOf("L", StringComparison.Ordinal)).Replace("7", "0");
                var tDate = new DateTime(currentYear, currentMonth, lastDayOfMonth);
                while ((Int32)tDate.DayOfWeek != Convert.ToInt32(weekday))
                {
                    tDate = tDate.AddDays(-1);
                }
                return date.Day == tDate.Day;
            }

            if (value.Contains("#"))
            {
                var weekdays = value.Split('#').Select(w => Convert.ToInt32(w)).ToArray();

                if (weekdays[0] == 7)
                {
                    weekdays[0] = 0;
                }

                if (weekdays[0] < 0 || weekdays[0] > 6)
                {
                    throw new ArgumentException("Weekday must be a value between 0 and 6.");
                }
                if (weekdays[1] > 5)
                {
                    throw new ArgumentException("There are never more than 5 of a given weekday in a month");
                }
                if ((Int32)date.DayOfWeek != weekdays[0])
                {
                    return false;
                }
                
                var tDate = new DateTime(currentYear, currentMonth, 1);
                var dayCount = 0;
                var currentDay = 1;

                while (currentDay < lastDayOfMonth + 1)
                {
                    if ((Int32) tDate.DayOfWeek == weekdays[0])
                    {
                        if (++dayCount >= weekdays[1])
                        {
                            break;
                        }
                    }
                    currentDay++;
                    tDate = tDate.AddDays(1);
                }
                return date.Day == currentDay;
            }
            if (value.Contains("-"))
            {
                var parts = value.Split('-');
                if (parts[0] == "7")
                {
                    parts[0] = "0";
                }
                else if (parts[1] == "0")
                {
                    parts[1] = "7";
                }
                value = string.Join("-", parts);
            }
            return IsSatisfied(((Int32)date.DayOfWeek).ToString(), value);
        }

        public override DateTime Increment(DateTime date)
        {
            var tDate = date.AddDays(-1);
            tDate = new DateTime(tDate.Year, tDate.Month, tDate.Day, 23, 59, 59);
            return tDate;
        }

        public override bool Validate(string value)
        {
            value = ConvertLiterals(value);
            if (value.Split(',').Any(val => !Regex.IsMatch(val, @"^(\*|[0-7](L?|#[1-5]))([\/\,\-][0-7]+)*$")) && !value.Equals("?"))
            {
                return false;
            }
            return true;
        }

        private string ConvertLiterals(string literals)
        {
            List<string> days = new List<string>() {"SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"};
            literals = days.Aggregate(literals, (current, day) => current.Replace(day, days.IndexOf(day).ToString()));
            return literals;
        }
    }
}
