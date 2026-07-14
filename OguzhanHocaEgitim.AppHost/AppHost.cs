var builder = DistributedApplication.CreateBuilder(args);


var app1 = builder.AddProject<Projects.AppDocker_API>("appdocker-api");


var app2 = builder.AddProject<Projects.AppDocker2_API>("appdocker2-api");


var redisPasswordParameter = builder.AddParameter("redispassword", "Password12*");

var sqlSever = builder.AddSqlServer("sqlserver").WithLifetime(ContainerLifetime.Persistent);


var redis = builder.AddRedis("redis").WithPassword(redisPasswordParameter);


//builder.AddContainer("sql-server", "mcr.microsoft.com/mssql/server:2025-latest").WithEnvironment("SA_PASSWORD",
//    "YourStrong!Passw0rd").WithEnvironment("ACCEPT_EULA", "Y")
//    .WithEndpoint(port: 1450, targetPort: 1433, name: "tcp");


app1.WithReference(app2).WaitFor(app2).WithReference(sqlSever).WaitFor(sqlSever);
app1.WithReference(redis).WaitFor(redis);


builder.Build().Run();
