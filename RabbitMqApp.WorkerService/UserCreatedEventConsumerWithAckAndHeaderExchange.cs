using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBusShared;

namespace RabbitMqApp.WorkerService;

// Headers Exchange dinleyicisi: sadece header'inda format=pdf olan mesajlari alir.
public class UserCreatedEventConsumerWithAckAndHeaderExchange(
    IConnection connection,
    ILogger<UserCreatedEventConsumerWithAckAndHeaderExchange> logger) :
    BackgroundService
{
    private const string ExchangeName = "rabbitmq-api.user-created-event.header-exchange";
    private const string QueueName = "worker-service.user-created-event-pdf.queue";

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UserCreatedEventConsumerWithAckAndHeaderExchange");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping UserCreatedEventConsumerWithAckAndHeaderExchange");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await channel.BasicQosAsync(0, 50, true, stoppingToken);

        await channel.QueueDeclareAsync(QueueName, true, false, false, cancellationToken: stoppingToken);

        // headers exchange'de routing key yerine binding argumanlari kullanilir
        // x-match = all  => tum header'lar eslesmeli (any => en az biri eslesse yeter)
        var bindingArguments = new Dictionary<string, object>
        {
            { "x-match", "all" },
            { "format", "pdf" },
            { "x", "y" }
        };

        await channel.QueueBindAsync(QueueName, ExchangeName, string.Empty, bindingArguments!,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                // header degerleri byte[] olarak gelir
                var format = args.BasicProperties.Headers?["format"] is byte[] formatAsBytes
                    ? Encoding.UTF8.GetString(formatAsBytes)
                    : "unknown";

                var message = Encoding.UTF8.GetString(args.Body.ToArray());

                var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                if (userCreatedEvent is null) logger.LogWarning("Received message is null or invalid.");

                logger.LogInformation($"Header exchange (format={format}): {userCreatedEvent!.UserId}," +
                                      $"{userCreatedEvent.UserName},{userCreatedEvent.Email}");

                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (Exception e)
            {
                await channel.BasicRejectAsync(args.DeliveryTag, true, stoppingToken);
                logger.LogError(e, e.Message);
            }
        };

        await channel.BasicConsumeAsync(QueueName, false, consumer, stoppingToken);
    }
}
