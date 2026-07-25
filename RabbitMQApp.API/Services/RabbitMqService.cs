using System.Text.Json;
using RabbitMQ.Client;
using ServiceBusShared;

namespace RabbitMQApp.API.Services;


public class RabbitMqServiecWithNoAspire
{
    public async Task BuidlConnection()
    {
        var rabbitMqConnection = "<connection string>";

        var factory = new ConnectionFactory
        {
            Uri = new Uri(rabbitMqConnection)
        };
        
        var factoryAsDocker= new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };
        
        using var connection = await factory.CreateConnectionAsync();

    }
    
}

public class RabbitMqService(IConnection connection)
{
    
    // at-most-once
    public async Task SendWithNoAck()
    {
        var channel =await connection.CreateChannelAsync();
   
        
        channel.BasicReturnAsync += (sender, args) =>
        {
            Console.WriteLine($"Message returned: {args.ReplyText}");
            return Task.CompletedTask;
        };
        
        // publisher =>  exchange
        // consumer =>  queue
        // <microservice_name>.<exchange_name>.<type>
       await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.exchange", ExchangeType.Fanout, true,
            false, null);


       var userCreatedEvent = new UserCreatedEvent(
           Guid.NewGuid().ToString(),
           "ahmet",
           "ahmet@outloo.com");

       var userCreatedEventAsJson= JsonSerializer.Serialize(userCreatedEvent);

       var body = System.Text.Encoding.UTF8.GetBytes(userCreatedEventAsJson);

       
        await channel.BasicPublishAsync("rabbitmq-api.user-created-event.exchange", string.Empty, true, body);
       
    
 
       
      await channel.DisposeAsync();


    }
    
    
    
    
    
    
    
}