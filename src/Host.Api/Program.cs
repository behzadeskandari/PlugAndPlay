using Framework.PluginLoader;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.WithProperty("Application", "PlugAndPlay.Host")
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();

PluginBootstrapper.AddPluginServices(builder.Services, builder.Configuration);

var serviceProvider = builder.Services.BuildServiceProvider();
var pluginContexts = PluginBootstrapper.LoadPlugins(builder.Environment.ContentRootPath, serviceProvider);

foreach (var pluginContext in pluginContexts)
{
    var pluginConfiguration = pluginContext.BuildPluginConfiguration(builder.Configuration, builder.Environment.EnvironmentName);
    builder.Services.AddPluginConfiguredServices(pluginContext, pluginConfiguration);
}

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthorization();

foreach (var pluginContext in pluginContexts)
{
    var pluginConfiguration = pluginContext.BuildPluginConfiguration(builder.Configuration, builder.Environment.EnvironmentName);
    app.UsePluginStartup(pluginContext, pluginConfiguration);
    app.UsePluginEndpoints(pluginContext, pluginConfiguration);
}

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.Run();
