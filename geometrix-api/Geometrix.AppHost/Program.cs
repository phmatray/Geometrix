var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Geometrix_WebApi>("geometrix")
    .WithExternalHttpEndpoints();

builder.Build().Run();