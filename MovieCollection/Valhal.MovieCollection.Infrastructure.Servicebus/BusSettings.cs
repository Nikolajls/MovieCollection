using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public class BusSettings
    {
        public TimeSpan Timeout { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool PublisherConfirms { get; set; }
        public string VirtualHost { get; set; }
        public int PrefetchCount { get; set; }
        public short HTTPPort { get; set; }

        public BusSettings()
        {
            Timeout = new TimeSpan(hours: 0, minutes: 0, seconds: 10);
            PublisherConfirms = false;
            VirtualHost = "/";
            PrefetchCount = 50;
            HTTPPort = 15672;
            Host = "localhost";
            Username = "guest";
            Password = "guest";
            /*
#if CONFIGRELEASE
            Host = "rmq.bws.dk";
            Username = "application";
            Password = "ready2play";
#elif CONFIGTEST || CONFIGSTAGING
            Host = "192.168.245.32";
            Username = "application";
            Password = "ready2play";
            HTTPPort = 80;
#if CONFIGTEST
            VirtualHost = "Test";
#elif CONFIGSTAGING
            VirtualHost = "Staging";
#endif
#elif CONFIGDEBUG
            Host = "localhost";
            Username = "guest";
            Password = "guest";
#endif
*/
        }
    }
}
