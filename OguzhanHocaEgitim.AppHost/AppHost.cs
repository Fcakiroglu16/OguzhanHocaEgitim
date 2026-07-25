var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.RabbitMQApp_API>("rabbitmqapp-api");

#pragma warning disable ASPIREPERSISTENCE001
var rabbitMq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin()
    .WithPersistentLifetime();
#pragma warning restore ASPIREPERSISTENCE001


api.WithReference(rabbitMq).WaitFor(rabbitMq);

builder.Build().Run();
