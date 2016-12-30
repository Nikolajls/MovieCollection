using System;
using System.Linq;
using FoxTales.Infrastructure.Extensions.Cron.Interface;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public abstract class AField : IField
    {
        public abstract bool IsSatisfiedBy(DateTime date, string value);
        public abstract DateTime Increment(DateTime date);
        public abstract bool Validate(string value);

        public bool IsSatisfied(string dateValue, string value)
        {
            if (IsIncrementsOfRanges(value))
            {
                return IsInIncrementsOfRanges(dateValue, value);
            }
            if (IsRange(value))
            {
                return IsInRange(dateValue, value);
            }
            return value == "*" || dateValue == value;
        }

        public bool IsRange(string value)
        {
            return value.Contains("-");
        }

        public bool IsIncrementsOfRanges(string value)
        {
            return value.Contains("/");
        }

        public bool IsInRange(string dateValue, string value)
        {
            var range = value.Split(new[] {'-'}, 2);
            var firstRange = Convert.ToInt32(range[0].Trim());
            var secondRange = Convert.ToInt32(range[1].Trim());
            var dateValueInt = Convert.ToInt32(dateValue);
            return dateValueInt >= firstRange && dateValueInt <= secondRange;
        }

        public bool IsInIncrementsOfRanges(string dateValue, string value)
        {
            var dateValueInt = Convert.ToInt32(dateValue);
            var parts = value.Split(new[] {'/'}, 2);
            var stepSize = parts.Count() > 1 ? (parts[1] != null ? Convert.ToInt32(parts[1].Trim()) : 0) : 0;

            var range = parts[0].Split(new[] {'-'}, 2);
            var offset = Convert.ToInt32(range[0].Trim());

            var to = range.Count() > 1
                ? (range[1] != null ? Convert.ToInt32(range[1].Trim()) : dateValueInt)
                : dateValueInt;

            if (dateValueInt < offset || dateValueInt > to)
            {
                return false;
            }

            if (dateValueInt > offset && stepSize == 0)
            {
                return false;
            }

            for (var i = offset; i <= to; i += stepSize)
            {
                if (i == dateValueInt)
                {
                    return true;
                }
            }
            return false;
        }
    }
}