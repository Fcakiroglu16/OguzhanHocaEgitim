var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Presentation_API>("presentation-api");

var api2 = builder.AddProject<Projects.WebApplication2_API>("webapplication2-api");

api.WithReference(api2);

builder.Build().Run();
