using RabbitMQ.Client;

namespace RabbitMQApp.API.Starter;

public static class ExchangeCreateStarter
{
    public static async Task CreateExchanges(this WebApplication web)
    {
        using var scope = web.Services.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
        var channel = await connection.CreateChannelAsync();
            
        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.exchange", ExchangeType.Fanout, true,
            false);
    }
    
}