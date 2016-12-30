using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.FluentConfiguration;
using EasyNetQ.Producer;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public class ValhalBus : IValhalBus
    {
        private readonly IBus _bus;
        private readonly IBusHelper _busHelper;
     
        public ValhalBus(IBus bus, IBusHelper busHelper)
        {
            _bus = bus;
            _busHelper = busHelper;
        }

        public void Dispose()
        {
            _bus.Dispose();
        }

        public void Publish<T>(T message) where T : class
        {
            _bus.Publish(message);
        }

        public void Publish<T>(T message, Action<IPublishConfiguration> configure) where T : class
        {
            Publish(message);
        }

        public void Publish<T>(T message, string topic) where T : class
        {
            _bus.Publish(message, topic);
        }

        public Task PublishAsync<T>(T message) where T : class
        {
            return _bus.PublishAsync(message);
        }

        public Task PublishAsync<T>(T message, Action<IPublishConfiguration> configure) where T : class
        {
            return PublishAsync(message);
        }

        public Task PublishAsync<T>(T message, string topic) where T : class
        {
            return _bus.PublishAsync(message, topic);
        }

        public ISubscriptionResult Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
        {
            return _bus.Subscribe(subscriptionId, onMessage);
        }

        public ISubscriptionResult Subscribe<T>(string subscriptionId, Action<T> onMessage, Action<ISubscriptionConfiguration> configure) where T : class
        {
            return _bus.Subscribe(subscriptionId, onMessage, configure);
        }

        public ISubscriptionResult SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
        {
            return _bus.SubscribeAsync(subscriptionId, onMessage);
        }

        public ISubscriptionResult SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage, Action<ISubscriptionConfiguration> configure) where T : class
        {
            return _bus.SubscribeAsync(subscriptionId, onMessage, configure);
        }

        public TResponse Request<TRequest, TResponse>(TRequest request) where TRequest : class where TResponse : class
        {
            return _bus.Request<TRequest, TResponse>(request);
        }

        public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : class where TResponse : class
        {
            return _bus.RequestAsync<TRequest, TResponse>(request);
        }

        public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder) where TRequest : class where TResponse : class
        {
            return _bus.Respond(responder);
        }

        public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder, Action<IResponderConfiguration> configure) where TRequest : class where TResponse : class
        {
            return _bus.Respond(responder, configure);
        }

        public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder) where TRequest : class where TResponse : class
        {
            return _bus.RespondAsync(responder);
        }

        public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder, Action<IResponderConfiguration> configure) where TRequest : class where TResponse : class
        {
            return _bus.RespondAsync(responder, configure);
        }

        public void Send<T>(string queue, T message) where T : class
        {
            _bus.Send(queue, message);
        }

        public Task SendAsync<T>(string queue, T message) where T : class
        {
            return _bus.SendAsync(queue, message);
        }

        public IDisposable Receive<T>(string queue, Action<T> onMessage) where T : class
        {
            return _bus.Receive(queue, onMessage);
        }

        public IDisposable Receive<T>(string queue, Action<T> onMessage, Action<IConsumerConfiguration> configure) where T : class
        {
            return _bus.Receive(queue, onMessage, configure);
        }

        public IDisposable Receive<T>(string queue, Func<T, Task> onMessage) where T : class
        {
            return _bus.Receive(queue, onMessage);
        }

        public IDisposable Receive<T>(string queue, Func<T, Task> onMessage, Action<IConsumerConfiguration> configure) where T : class
        {
            return _bus.Receive(queue, onMessage, configure);
        }

        public IDisposable Receive(string queue, Action<IReceiveRegistration> addHandlers)
        {
            return _bus.Receive(queue, addHandlers);
        }

        public IDisposable Receive(string queue, Action<IReceiveRegistration> addHandlers, Action<IConsumerConfiguration> configure)
        {
            return _bus.Receive(queue, addHandlers, configure);
        }

        public bool IsConnected => _bus.IsConnected;

        public IAdvancedBus Advanced => _bus.Advanced;

        public void FuturePublish<T>(DateTime futurePublishDate, T message) where T : class
        {
            _busHelper.FuturePublish(futurePublishDate, message);
        }

        public void FuturePublish<T>(DateTime futurePublishDate, string cancellationKey, T message) where T : class
        {
            _busHelper.FuturePublish(futurePublishDate, cancellationKey, message);
        }

        public void FuturePublish<T>(TimeSpan messageDelay, T message) where T : class
        {
            _busHelper.FuturePublish(messageDelay, message);
        }

        public Task FuturePublishAsync<T>(DateTime futurePublishDate, T message) where T : class
        {
            return _busHelper.FuturePublishAsync(futurePublishDate, message);
        }

        public Task FuturePublishAsync<T>(DateTime futurePublishDate, string cancellationKey, T message) where T : class
        {
            return _busHelper.FuturePublishAsync(futurePublishDate, cancellationKey, message);
        }

        public Task FuturePublishAsync<T>(TimeSpan messageDelay, T message) where T : class
        {
            return _busHelper.FuturePublishAsync(messageDelay, message);
        }

        public void CancelFuturePublish(string cancellationKey)
        {
            _busHelper.CancelFuturePublish(cancellationKey);
        }

        public Task CancelFuturePublishAsync(string cancellationKey)
        {
            return _busHelper.CancelFuturePublishAsync(cancellationKey);
        }

    }
}
