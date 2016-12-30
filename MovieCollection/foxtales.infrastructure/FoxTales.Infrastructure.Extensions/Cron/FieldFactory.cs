using FoxTales.Infrastructure.Extensions.Cron.Interface;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public class FieldFactory
    {
        private IField[] _fields = new IField[7];

        public IField GetField(int position)
        {
            if (_fields[position] == null)
            {
                switch (position)
                {
                    case 0:
                        _fields[position] = new SecondsField();
                        break;
                    case 1:
                        _fields[position] = new MinutesField();
                        break;
                    case 2:
                        _fields[position] = new HoursField();
                        break;
                    case 3:
                        _fields[position] = new DayOfMonthField();
                        break;
                    case 4:
                        _fields[position] = new MonthsField();
                        break;
                    case 5:
                        _fields[position] = new DayOfWeekField();
                        break;
                    case 6:
                        _fields[position] = new YearsField();
                        break;
                }
            }
            return _fields[position];
        }
    }
}
