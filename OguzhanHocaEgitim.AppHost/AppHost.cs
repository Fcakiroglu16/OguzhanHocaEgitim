var builder = DistributedApplication.CreateBuilder(args);

var workerService=builder.AddProject<Projects.RabbitMqApp_WorkerService>("rabbitmqapp-workerservice");

var api = builder.AddProject<Projects.RabbitMQApp_API>("rabbitmqapp-api");





var rabbitMqUserName=builder.AddParameter("username", "guest");
var rabbitMqPassword=builder.AddParameter("password", "Password12*");


#pragma warning disable ASPIREPERSISTENCE001
var rabbitMq = builder.AddRabbitMQ("rabbitmq", rabbitMqUserName, rabbitMqPassword).WithManagementPlugin()
    .WithPersistentLifetime();
#pragma warning restore ASPIREPERSISTENCE001


api.WithReference(rabbitMq).WaitFor(rabbitMq);

workerService.WithReference(rabbitMq).WaitFor(rabbitMq);


builder.Build().Run();
