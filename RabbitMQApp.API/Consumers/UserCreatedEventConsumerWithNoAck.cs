using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBusShared;

namespace RabbitMQApp.API.Consumers;

public class UserCreatedEventConsumerWithNoAck(IConnection connection,ILogger<UserCreatedEventConsumerWithNoAck> logger) : BackgroundService
{
    // hooks

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        
        logger.LogInformation("Starting UserCreatedEventConsumerWithNoAck");
        return base.StartAsync(cancellationToken);
    }
//gracefull shot down
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        
        logger.LogInformation("Stopping UserCreatedEventConsumer");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        // exchange must exist before binding; declare is idempotent and must match the publisher
        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.exchange", ExchangeType.Fanout, true,
            false, cancellationToken: stoppingToken);


        // queue name =>  <microservice-name>.<queue-name>.<message-type>
        await channel.QueueDeclareAsync("rabbitmq-api.user-created-event.queue", true, false, false,
            cancellationToken:
            stoppingToken);


        await channel.QueueBindAsync("rabbitmq-api.user-created-event.queue",
            "rabbitmq-api.user-created-event.exchange", string.Empty, cancellationToken: stoppingToken);


        var consumer = new AsyncEventingBasicConsumer(channel);


        consumer.ReceivedAsync += (sender, args) =>
        {
            
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                UserCreatedEvent?  userCreatedEvent= JsonSerializer.Deserialize<UserCreatedEvent>(message);
                
                if(userCreatedEvent is null)
                {
                    logger.LogWarning("Received message is null or invalid.");
                    return Task.CompletedTask;
                }
                
                logger.LogInformation($"Received message: {userCreatedEvent!.UserId},{userCreatedEvent.UserName}," + 
                    $"{userCreatedEvent.Email}");
                return Task.CompletedTask;
            
          
        };


        await channel.BasicConsumeAsync("rabbitmq-api.user-created-event.queue", true, consumer, cancellationToken: 
            stoppingToken);


     
    }
}