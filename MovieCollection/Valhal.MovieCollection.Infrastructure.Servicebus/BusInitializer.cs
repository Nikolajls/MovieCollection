using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public class BusInitializer
    {
        public static IBus CreateBus(BusSettings settings = null)
        {
            settings = settings ?? new BusSettings();
            var bus = RabbitHutch.CreateBus(string.Format("host={1};username={2};password={3};timeout={0};publisherConfirms={4};virtualHost={5};prefetchcount={6}", settings.Timeout.TotalSeconds, settings.Host, settings.Username, settings.Password, settings.PublisherConfirms.ToString().ToLower(), settings.VirtualHost, settings.PrefetchCount));
            return bus;
        }
    }

}
