using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentBikeApi.Domain.Events;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Infrastructure.Messaging
{
    public sealed class MotorcycleRegisteredConsumer(
        IOptions<RabbitMqOptions> options,
        IServiceScopeFactory scopeFactory,
        ILogger<MotorcycleRegisteredConsumer> logger
        ) : BackgroundService
    {
        private const string QueueName = nameof(MotorcycleRegisteredEvent);
        private IConnection? _connection;
        private IChannel? _channel;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var settings = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = settings.Host,
                UserName = settings.User,
                Password = settings.Password
            };
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += HandleMessageAsync;

            await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            logger.LogInformation("Listening for messages on queue {Queue}", QueueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs args)
        {
            try
            {
                var @event = JsonSerializer.Deserialize<MotorcycleRegisteredEvent>(args.Body.Span);
                if(@event is not null)
                {
                    await using var scope = scopeFactory.CreateAsyncScope();
                    var service = scope.ServiceProvider.GetRequiredService<IMotorcycleNotificationService>();
                    var stored = await service.HandleMotorcycleRegisteredAsync(@event);
                    logger.LogInformation("Received {EventType} for motorcycle {MotorcycleId} (year {Year}), Notification stored: {Stored}", QueueName, @event.MotorcycleId, @event.Year, stored);
                }
                await _channel!.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process message {DeliveryTag} from {Queue}", args.DeliveryTag, QueueName);
                await _channel!.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: false);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null) await _channel.DisposeAsync();
            if (_connection is not null) await _connection.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
