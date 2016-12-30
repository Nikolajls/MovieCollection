using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public interface IBusHelper
    {
        void FuturePublish<T>(DateTime futurePublishDate, T message) where T : class;
        void FuturePublish<T>(DateTime futurePublishDate, string cancellationKey, T message) where T : class;
        void FuturePublish<T>(TimeSpan messageDelay, T message) where T : class;
        Task FuturePublishAsync<T>(DateTime futurePublishDate, T message) where T : class;
        Task FuturePublishAsync<T>(DateTime futurePublishDate, string cancellationKey, T message) where T : class;
        Task FuturePublishAsync<T>(TimeSpan messageDelay, T message) where T : class;
        void CancelFuturePublish(string cancellationKey);
        Task CancelFuturePublishAsync(string cancellationKey);
    }
}
