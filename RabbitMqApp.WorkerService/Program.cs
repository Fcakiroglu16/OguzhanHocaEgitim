using RabbitMqApp.WorkerService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

builder.Services.AddHostedService<UserCreatedEventConsumerWithAck>();
builder.Services.AddHostedService<UserCreatedEventConsumerWithAckAndDirectExchange>();
var host = builder.Build();
host.Run();