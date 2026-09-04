using RabbitMQApp.API.Consumers;
using RabbitMQApp.API.Services;
using RabbitMQApp.API.Starter;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQClient("rabbitmq");
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddOpenApi();
//builder.Services.AddHostedService<UserCreatedEventConsumerWithNoAck>();
builder.Services.AddHostedService<UserCreatedEventConsumerWithAck>();


var app = builder.Build();

await app.CreateExchanges();


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();


app.MapGet("/send-with-no-ack", async (RabbitMqService rabbitMqService) =>
{
    await rabbitMqService.SendWithNoAck();

    return Results.Ok();
});
app.MapGet("/send-with-ack", async (RabbitMqService rabbitMqService) =>
{
    await rabbitMqService.SendWithAckAndTopicExchangeWithHeader();

    return Results.Ok();
});

// ornek: /send-with-header-exchange?format=pdf  => consumer alir
//        /send-with-header-exchange?format=excel => eslesme yok, mesaj geri doner
app.MapGet("/send-with-header-exchange", async (RabbitMqService rabbitMqService, string format = "pdf") =>
{
    await rabbitMqService.SendWithAckAndHeaderExchange(format);

    return Results.Ok();
});
app.MapGet("/send-with-dead-letter-exchange", async (RabbitMqService rabbitMqService, string format = "pdf") =>
{
    await rabbitMqService.SendWithAckAndDeadLetterExchange();

    return Results.Ok();
});
app.Run();