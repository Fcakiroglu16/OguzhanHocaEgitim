using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBusShared;
using System.Text;
using System.Text.Json;

namespace RabbitMqApp.WorkerService;

public class UserCreatedEventConsumerWithDeadLetterExchange(IConnection connection, 
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
        await channel.BasicQosAsync(0, 50, true, cancellationToken: stoppingToken);
        // exchange must exist before binding; declare is idempotent and must match the publisher
        
        
        
        //dead letter exchange

       await  channel.ExchangeDeclareAsync("worker-service.user-created-event.dlx", ExchangeType.Fanout, true, cancellationToken: stoppingToken);
        
       
       await channel.QueueDeclareAsync("worker-service.user-created-event.error-dlq", true, false, false,
            cancellationToken: stoppingToken);
        
        
       await channel.QueueBindAsync("worker-service.user-created-event.error-dlq", "worker-service.user-created-event.dlx", string.Empty, cancellationToken: stoppingToken);
        
        
        
        
        
       var argumens= new Dictionary<string, object>
       {
           {"x-dead-letter-exchange", "worker-service.user-created-event.dlx"},
           {"x-message-ttl", 5000}, // message TTL in milliseconds
           {"x-max-length", 100} // max 100 message
           
           
           
           
       };
        
 
       await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.exchange", ExchangeType.Fanout, true,
           false);
       
        await channel.QueueDeclareAsync("worker-service.user-created-event.queue", true, false, false, argumens!,
            cancellationToken:
            stoppingToken);
        
        await channel.QueueBindAsync("worker-service.user-created-event.queue",
            "rabbitmq-api.user-created-event.exchange", string.Empty, cancellationToken: stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);


        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                
                throw new Exception("db hatası");
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                UserCreatedEvent? userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                if (userCreatedEvent is null)
                {
                    logger.LogWarning("Received message is null or invalid.");

                }

                logger.LogInformation($"Received message 3: {userCreatedEvent!.UserId},{userCreatedEvent.UserName}," +
                                      $"{userCreatedEvent.Email}");

                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (Exception e)
            {
                
                await channel.BasicRejectAsync(args.DeliveryTag, false, stoppingToken);
                logger.LogError(e, e.Message);
            }

        };


        await channel.BasicConsumeAsync("worker-service.user-created-event.queue", false, consumer, cancellationToken:
            stoppingToken);



    }
}