using System.Text;
using System.Text.Json;
using Polly;
using Polly.Retry;
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

        var factoryAsDocker = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };

        using var connection = await factory.CreateConnectionAsync();
    }
}

public class RabbitMqService(IConnection connection, ILogger<RabbitMqService> logger)
{
    // at-most-once = no retry = no ack
    public async Task SendWithNoAck()
    {
        var channel = await connection.CreateChannelAsync();


        channel.BasicReturnAsync += (sender, args) =>
        {
            logger.LogInformation($"Message returned: {args.ReplyText}");
            return Task.CompletedTask;
        };

        // publisher =>  exchange
        // consumer =>  queue
        // <microservice_name>.<exchange_name>.<type>
        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.exchange", ExchangeType.Fanout, true,
            false);


        var userCreatedEvent = new UserCreatedEvent(
            Guid.NewGuid().ToString(),
            "ahmet",
            "ahmet@outloo.com");

        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);

        var body = Encoding.UTF8.GetBytes(userCreatedEventAsJson);


        await channel.BasicPublishAsync("rabbitmq-api.user-created-event.exchange", string.Empty, true, body);


        await channel.DisposeAsync();
    }


    // at-least-once = yes retry = yes ack
    public async Task SendWithAck()
    {
        var channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true));

        channel.BasicReturnAsync += (sender, args) =>
        {
            logger.LogInformation($"Message returned: {args.ReplyText}");
            return Task.CompletedTask;
        };

        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.exchange", ExchangeType.Fanout, true,
            false);


        var userCreatedEvent = new UserCreatedEvent(
            Guid.NewGuid().ToString(),
            "ahmet",
            "ahmet@outloo.com");

        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);

        var body = Encoding.UTF8.GetBytes(userCreatedEventAsJson);


        var retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning(
                        args.Outcome.Exception,
                        "BasicPublishAsync failed, retry attempt {AttemptNumber}",
                        args.AttemptNumber + 1);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        await retryPipeline.ExecuteAsync(async cancellationToken =>
        {
            await channel.BasicPublishAsync("rabbitmq-api.user-created-event.exchange", string.Empty, true, body,
                cancellationToken);
        });


        await channel.DisposeAsync();
    }

    public async Task SendWithNoAck2()
    {
        var channel = await connection.CreateChannelAsync();


        channel.BasicReturnAsync += (sender, args) =>
        {
            Console.WriteLine($"Message returned: {args.ReplyText}");
            return Task.CompletedTask;
        };

        // publisher =>  exchange
        // consumer =>  queue
        // <microservice_name>.<exchange_name>.<type>
        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.exchange", ExchangeType.Fanout, true,
            false);


        var userCreatedEvent = new UserCreatedEvent(
            Guid.NewGuid().ToString(),
            "ahmet",
            "ahmet@outloo.com");

        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);

        var body = Encoding.UTF8.GetBytes(userCreatedEventAsJson);


        var publish = channel.BasicPublishAsync("rabbitmq-api.user-created-event.exchange", string.Empty, true, body);

        publish.AsTask().ContinueWith(async task =>
        {
            if (task.IsFaulted) logger.LogError(task.Exception, task.Exception.Message);

            await channel.DisposeAsync();
        });
    }

    public async Task SendWithAckAndDirectExchange()
    {
        var channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true));

        channel.BasicReturnAsync += (sender, args) =>
        {
            logger.LogInformation($"Message returned: {args.ReplyText}");
            return Task.CompletedTask;
        };

        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.direct-exchange", ExchangeType.Direct, true,
            false);


        var userCreatedEvent = new UserCreatedEvent(
            Guid.NewGuid().ToString(),
            "ahmet",
            "ahmet@outloo.com");

        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);

        var body = Encoding.UTF8.GetBytes(userCreatedEventAsJson);


        var retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning(
                        args.Outcome.Exception,
                        "BasicPublishAsync failed, retry attempt {AttemptNumber}",
                        args.AttemptNumber + 1);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        //await retryPipeline.ExecuteAsync(async cancellationToken =>
        //{
        //    await channel.BasicPublishAsync("rabbitmq-api.user-created-event.direct-exchange", "type:developer", true, body,
        //        cancellationToken);
        //});
        await retryPipeline.ExecuteAsync(async cancellationToken =>
        {
            await channel.BasicPublishAsync("rabbitmq-api.user-created-event.direct-exchange", "type:developer", true,
                body,
                cancellationToken);
        });

        await channel.DisposeAsync();
    }

    public async Task SendWithAckAndTopicExchange()
    {
        var channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true));

        channel.BasicReturnAsync += (sender, args) =>
        {
            logger.LogInformation($"Message returned: {args.ReplyText}");
            return Task.CompletedTask;
        };

        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.topic-exchange", ExchangeType.Topic, true,
            false);


        var userCreatedEvent = new UserCreatedEvent(
            Guid.NewGuid().ToString(),
            "ahmet",
            "ahmet@outloo.com");

        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);

        var body = Encoding.UTF8.GetBytes(userCreatedEventAsJson);


        var retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning(
                        args.Outcome.Exception,
                        "BasicPublishAsync failed, retry attempt {AttemptNumber}",
                        args.AttemptNumber + 1);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        //await retryPipeline.ExecuteAsync(async cancellationToken =>
        //{
        //    await channel.BasicPublishAsync("rabbitmq-api.user-created-event.direct-exchange", "type:developer", true, body,
        //        cancellationToken);
        //});
        await retryPipeline.ExecuteAsync(async cancellationToken =>
        {
            await channel.BasicPublishAsync("rabbitmq-api.user-created-event.topic-exchange", "dev.senior.developer",
                false,
                body,
                cancellationToken);
        });

        await channel.DisposeAsync();
    }

    public async Task SendWithAckAndTopicExchangeWithHeader()
    {
        var channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true));

        channel.BasicReturnAsync += (sender, args) =>
        {
            logger.LogInformation($"Message returned: {args.ReplyText}");
            return Task.CompletedTask;
        };

        await channel.ExchangeDeclareAsync("rabbitmq-api.user-created-event.topic-exchange", ExchangeType.Topic, true,
            false);


        var userCreatedEvent = new UserCreatedEvent(
            Guid.NewGuid().ToString(),
            "ahmet",
            "ahmet@outloo.com");


        var properties = new BasicProperties();

        // add header
        properties.Headers = new Dictionary<string, object>
        {
            { "version", 1 },
            { "created", DateTime.UtcNow.ToShortDateString() }
        }!;

        properties.Persistent = true;
        // add timestamp
        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        properties.CorrelationId = Guid.NewGuid().ToString();
        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);

        var body = Encoding.UTF8.GetBytes(userCreatedEventAsJson);


        var retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning(
                        args.Outcome.Exception,
                        "BasicPublishAsync failed, retry attempt {AttemptNumber}",
                        args.AttemptNumber + 1);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        //await retryPipeline.ExecuteAsync(async cancellationToken =>
        //{
        //    await channel.BasicPublishAsync("rabbitmq-api.user-created-event.direct-exchange", "type:developer", true, body,
        //        cancellationToken);
        //});
        await retryPipeline.ExecuteAsync(async cancellationToken =>
        {
            await channel.BasicPublishAsync("rabbitmq-api.user-created-event.topic-exchange", "dev.senior.developer",
                false,
                properties, body,
                cancellationToken);
        });

        await channel.DisposeAsync();
    }
}