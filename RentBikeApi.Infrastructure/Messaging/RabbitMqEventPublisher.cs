using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Infrastructure.Messaging
{
    public sealed class RabbitMqEventPublisher(IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqEventPublisher> logger) : IEventPublisher, IAsyncDisposable
    {
        private readonly SemaphoreSlim _lock = new(1, 1);
        private IConnection? _connection;
        private IChannel? _channel;

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            var queueName = typeof(TEvent).Name;
            var body = JsonSerializer.SerializeToUtf8Bytes(@event);
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };
            await _lock.WaitAsync();
            try
            {
                var channel = await GetChannelAsync();
                await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);
                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: queueName,
                    mandatory: false,
                    basicProperties: properties,
                    body: body);
            }
            finally
            {
                _lock.Release();
            }
            logger.LogInformation("Published {EventType} to queue {Queue}", typeof(TEvent).Name, queueName);
        }

        private async Task<IChannel> GetChannelAsync()
        {
            if (_channel is { IsOpen: true })
                return _channel;

            var settings = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = settings.Host,
                UserName = settings.User,
                Password = settings.Password
            };
            _connection ??= await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            return _channel;
        }
        public async ValueTask DisposeAsync()
        {
            if (_channel is not null) await _channel.DisposeAsync();
            if (_connection is not null) await _connection.DisposeAsync();
            _lock.Dispose();
        }
    }
}
