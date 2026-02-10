using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var postgresdb = builder.AddPostgres("pg").AddDatabase("postgresdb");

var server = builder.AddProject<Projects.MSK_Končar_WeatherApp_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

var scalar = builder.AddScalarApiReference();
scalar.WithApiReference(server);

builder.Build().Run();
