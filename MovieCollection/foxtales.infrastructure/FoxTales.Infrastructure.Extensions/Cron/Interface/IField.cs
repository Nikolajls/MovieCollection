using System;

namespace FoxTales.Infrastructure.Extensions.Cron.Interface
{
    public interface IField
    {
        bool IsSatisfiedBy(DateTime date, string value);
        DateTime Increment(DateTime date);
        bool Validate(string value);
    }
}
