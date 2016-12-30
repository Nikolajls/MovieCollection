using System;

namespace FoxTales.Infrastructure.Extensions.Cron
{
    public static class CronScheduler
    {
        public static DateTime CalculatePreviousCronRun(this string cronString)
        {
            return new CronExpression(cronString).GetPreviousRunDate();
        }
    }
}