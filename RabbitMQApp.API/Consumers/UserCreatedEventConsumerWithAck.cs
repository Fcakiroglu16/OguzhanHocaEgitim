using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBusShared;
using System.Text;
using System.Text.Json;

namespace RabbitMQApp.API.Consumers;

public class UserCreatedEventConsumerWithAck(IConnection connection, ILogger<UserCreatedEventConsumerWithAck> logger) :
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

        await channel.BasicQosAsync(0, 50, true, cancellationToken: stoppingToken);




        // queue name =>  <microservice-name>.<queue-name>.<message-type>
        await channel.QueueDeclareAsync("rabbitmq-api.user-created-event.queue", true, false, false,
            cancellationToken:
            stoppingToken);


        await channel.QueueBindAsync("rabbitmq-api.user-created-event.queue",
            "rabbitmq-api.user-created-event.exchange", string.Empty, cancellationToken: stoppingToken);


        var consumer = new AsyncEventingBasicConsumer(channel);


        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                UserCreatedEvent? userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                if (userCreatedEvent is null)
                {
                    logger.LogWarning("Received message is null or invalid.");

                }

                logger.LogInformation($"Received message 2: {userCreatedEvent!.UserId},{userCreatedEvent.UserName}," +
                                      $"{userCreatedEvent.Email}");

                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (Exception e)
            {
                await channel.BasicRejectAsync(args.DeliveryTag, true, stoppingToken);
                logger.LogError(e, e.Message);
            }

        };


        await channel.BasicConsumeAsync("rabbitmq-api.user-created-event.queue", false, consumer, cancellationToken:
            stoppingToken);



    }
}