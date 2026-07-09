var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.AppDocker_API>("appdocker-api");


builder.AddProject<Projects.AppDocker2_API>("appdocker2-api");


builder.Build().Run();
