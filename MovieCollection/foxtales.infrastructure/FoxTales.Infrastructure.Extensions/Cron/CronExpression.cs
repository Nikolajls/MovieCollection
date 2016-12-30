using System;
using System.Linq;
using FoxTales.Infrastructure.Extensions.Cron.Interface;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class CronExpression
    {
        public const int Second = 0;
        public const int Minute = 1;
        public const int Hour = 2;
        public const int DayOfMonth = 3;
        public const int Month = 4;
        public const int Weekday = 5;
        public const int Year = 6;

        private string[] _cronParts;
        private FieldFactory _fieldFactory = new FieldFactory();
        private static int[] _order = new[] {Year, Month, DayOfMonth, Weekday, Hour, Minute, Second};


        public CronExpression(string expression)
        {
            SetExpression(expression);
        }

        public void SetExpression(string value)
        {
            _cronParts = value.Split(new[] {@" "}, StringSplitOptions.RemoveEmptyEntries);
            if (_cronParts.Count() < 6)
            {
                throw new ArgumentException(value + " is not a valid CRON epxression.");
            }

            foreach (var cronPart in _cronParts)
            {
                SetPart(Array.IndexOf(_cronParts, cronPart), cronPart);
            }
        }

        private void SetPart(int position, string value)
        {
            if (!_fieldFactory.GetField(position).Validate(value))
            {
                throw new ArgumentException("Invalid CRON field value " + value + " at position " + position);
            }
            _cronParts[position] = value;
        }

        public DateTime GetPreviousRunDate()
        {
            var currentTime = DateTime.Now;
            var nth = 0;
            var nextRun = currentTime;
            var loopBreaked = false;
            var parts = new string[7];
            var fields = new IField[7];

            foreach (var position in _order)
            {
                var part = GetExpression(position);
                if (part == null || part == "*")
                {
                    continue;
                }
                parts[position] = part;
                fields[position] = _fieldFactory.GetField(position);
            }
             
            for (int i = 0; i < 10000000; i++)
            {
                loopBreaked = false;
                foreach (var part in parts)
                {
                    var satisfied = false;
                    var field = fields[Array.IndexOf(parts, part)];
                    if (part != null && !part.Contains(","))
                    {
                        satisfied = field.IsSatisfiedBy(nextRun, part);
                    }
                    else if(part != null)
                    {
                        var listParts = part.Split(',');
                        foreach (var listPart in listParts)
                        {
                            satisfied = field.IsSatisfiedBy(nextRun, listPart.Trim());
                            if (satisfied)
                                break;
                        }
                    }
                    if (field != null && !satisfied)
                    {
                        nextRun = field.Increment(nextRun);
                        loopBreaked = true;
                        break;
                    }
                }
                if (loopBreaked)
                {
                    continue;
                }
                if (nextRun == currentTime || --nth > -1)
                {
                    nextRun = _fieldFactory.GetField(0).Increment(nextRun);
                    continue;
                }
                return nextRun;
            }
            throw new Exception("Impossible CRON expression");
        }

        public string GetExpression(int position = -1)
        {
            if (position == -1)
            {
                return string.Join(" ", _cronParts);
            }
            if (_cronParts[position] != null)
            {
                return _cronParts[position];
            }
            return null;
        }

        public string CronToString()
        {
            return GetExpression();
        }

    }
}
