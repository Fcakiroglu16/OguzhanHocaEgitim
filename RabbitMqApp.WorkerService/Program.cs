using RabbitMqApp.WorkerService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

// builder.Services.AddHostedService<UserCreatedEventConsumerWithAck>();
// builder.Services.AddHostedService<UserCreatedEventConsumerWithAckAndDirectExchange>();
// builder.Services.AddHostedService<UserCreatedEventConsumerWithAckAndTopicExchange>();
//builder.Services.AddHostedService<UserCreatedEventConsumerWithAckAndHeaderExchange>();
builder.Services.AddHostedService<UserCreatedEventConsumerWithDeadLetterExchange>();
var host = builder.Build();
host.Run();