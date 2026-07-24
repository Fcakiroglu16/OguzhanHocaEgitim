var builder = DistributedApplication.CreateBuilder(args);

var rabbitmqapi = builder.AddProject<Projects.RabbitMQApp_API>("rabbitmqapp-api");

var rabbitMq = builder.AddRabbitMQ("rabbitmq");

rabbitmqapi.WithReference(rabbitMq).WaitFor(rabbitMq);

builder.Build().Run();
