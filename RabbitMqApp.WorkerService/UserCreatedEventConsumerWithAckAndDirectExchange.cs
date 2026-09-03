using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBusShared;

namespace RabbitMqApp.WorkerService;

public class UserCreatedEventConsumerWithAckAndDirectExchange(
    IConnection connection,
    ILogger<UserCreatedEventConsumerWithAck> logger) :
    BackgroundService
{
    // hooks

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UserCreatedEventConsumer");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping UserCreatedEventConsumer");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await channel.BasicQosAsync(0, 50, true, stoppingToken);
        // exchange must exist before binding; declare is idempotent and must match the publisher
        // await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.direct-exchange", ExchangeType.Direct, true,
        //     false, cancellationToken: stoppingToken);


        // queue name =>  <microservice-name>.<queue-name>.<message-type>
        await channel.QueueDeclareAsync("worker-service.user-created-event2.queue", true, false, false,
            cancellationToken:
            stoppingToken);


        await channel.QueueBindAsync("worker-service.user-created-event2.queue",
            "rabbitmq-api.user-created-event.direct-exchange", "type:developer", cancellationToken: stoppingToken);


        var consumer = new AsyncEventingBasicConsumer(channel);


        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                if (userCreatedEvent is null) logger.LogWarning("Received message is null or invalid.");

                logger.LogInformation($"Received message 3: {userCreatedEvent!.UserId},{userCreatedEvent.UserName}," +
                                      $"{userCreatedEvent.Email}");

                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (Exception e)
            {
                await channel.BasicRejectAsync(args.DeliveryTag, true, stoppingToken);
                logger.LogError(e, e.Message);
            }
        };


        await channel.BasicConsumeAsync("worker-service.user-created-event2.queue", false, consumer, stoppingToken);
    }
}