using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Scheduling;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public class BusHelper : IBusHelper
    {
        private readonly IBus _bus;

        public BusHelper(IBus bus)
        {
            _bus = bus;
        }

        public void FuturePublish<T>(DateTime futurePublishDate, T message) where T : class
        {
            _bus.FuturePublish(futurePublishDate, message);
        }

        public void FuturePublish<T>(DateTime futurePublishDate, string cancellationKey, T message) where T : class
        {
            _bus.FuturePublish(futurePublishDate, cancellationKey, message);
        }

        public void FuturePublish<T>(TimeSpan messageDelay, T message) where T : class
        {
            _bus.FuturePublish(messageDelay, message);
        }

        public Task FuturePublishAsync<T>(DateTime futurePublishDate, T message) where T : class
        {
            return _bus.FuturePublishAsync(futurePublishDate, message);
        }

        public Task FuturePublishAsync<T>(DateTime futurePublishDate, string cancellationKey, T message) where T : class
        {
            return _bus.FuturePublishAsync(futurePublishDate, cancellationKey, message);
        }

        public Task FuturePublishAsync<T>(TimeSpan messageDelay, T message) where T : class
        {
            return _bus.FuturePublishAsync(messageDelay, message);
        }

        public void CancelFuturePublish(string cancellationKey)
        {
            _bus.CancelFuturePublish(cancellationKey);
        }

        public Task CancelFuturePublishAsync(string cancellationKey)
        {
            return _bus.CancelFuturePublishAsync(cancellationKey);
        }
    }
}
