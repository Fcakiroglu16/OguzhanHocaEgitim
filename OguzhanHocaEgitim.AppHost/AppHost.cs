using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var workerService = builder.AddProject<RabbitMqApp_WorkerService>("rabbitmqapp-workerservice").WithReplicas(1);

var api = builder.AddProject<RabbitMQApp_API>("rabbitmqapp-api").WithReplicas(1);


var rabbitMqUserName = builder.AddParameter("username", "guest");
var rabbitMqPassword = builder.AddParameter("password", "Password12*");


#pragma warning disable ASPIREPERSISTENCE001
var rabbitMq = builder.AddRabbitMQ("rabbitmq", rabbitMqUserName, rabbitMqPassword)
    .WithManagementPlugin()
    // .WithEndpoint(port: 6672, targetPort: 5672, name: "amqp")
    // .WithEndpoint(port: 25672, targetPort: 15672, name: "management")
    .WithPersistentLifetime();
#pragma warning restore ASPIREPERSISTENCE001

api.WithReference(rabbitMq).WaitFor(rabbitMq);

workerService.WithReference(rabbitMq).WaitFor(api);


builder.Build().Run();