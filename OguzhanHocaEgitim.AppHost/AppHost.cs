var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Presentation_API>("presentation-api");

//http://webapplication2-api/api/order
//http://localhost:2000/api/order
var api2 = builder.AddProject<Projects.WebApplication2_API>("webapplication2-api");

api.WithReference(api2);

api2.WithReference(api);

builder.AddProject<Projects.CacheLesson_API>("cachelesson-api");

builder.AddProject<Projects.AppDocker_API>("appdocker-api");

builder.Build().Run();
