using RabbitMQApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQClient("rabbitmq");
builder.Services.AddSingleton<RabbitMqService>();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapGet("/send-with-no-ack", async (RabbitMqService rabbitMqService) =>
{
    await rabbitMqService.SendWithNoAck();

    return Results.Ok();


});



app.Run();

