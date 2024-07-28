using Serilog;

using AsaasBlazorAuthentication.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Add services to the container.
    builder.ConfigureServices();

    var app = builder.Build();
    // Configure the HTTP request pipeline.
    app.ConfigureApplication();

    Log.Information("Application starting.");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application has found an error in runtime.");
}
finally
{
    Log.CloseAndFlush();
}