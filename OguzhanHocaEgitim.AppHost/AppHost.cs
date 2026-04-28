var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Presentation_API>("presentation-api");

builder.AddProject<Projects.WebApplication2_API>("webapplication2-api");

builder.Build().Run();
