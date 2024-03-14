var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Geometrix_WebApi>("Geometrix.WebApi");

builder.Build().Run();