using Framework.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Framework.PluginLoader;

public static class PluginExtensions
{
    public static IServiceCollection AddPluginConfiguredServices(this IServiceCollection services, PluginContext pluginContext, IConfiguration configuration)
    {
        var startupImplementations = pluginContext.PluginAssembly.GetTypes()
            .Where(type => typeof(IPluginStartup).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            .Select(type => (IPluginStartup)Activator.CreateInstance(type)!)
            .ToArray();

        services.AddApplicationLayer(pluginContext.PluginAssembly);

        foreach (var startup in startupImplementations)
        {
            startup.ConfigureServices(services, configuration);
        }

        return services;
    }

    public static IEndpointRouteBuilder UsePluginEndpoints(this IEndpointRouteBuilder endpoints, PluginContext pluginContext, IConfiguration configuration)
    {
        var endpointTypes = pluginContext.PluginAssembly.GetTypes()
            .Where(type => typeof(IPluginEndpoint).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            .Select(type => (IPluginEndpoint)Activator.CreateInstance(type)!)
            .ToArray();

        foreach (var endpoint in endpointTypes)
        {
            endpoint.MapEndpoints(endpoints);
        }

        return endpoints;
    }

    public static Microsoft.AspNetCore.Builder.IApplicationBuilder UsePluginStartup(this Microsoft.AspNetCore.Builder.IApplicationBuilder app, PluginContext pluginContext, IConfiguration configuration)
    {
        var startupImplementations = pluginContext.PluginAssembly.GetTypes()
            .Where(type => typeof(IPluginStartup).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            .Select(type => (IPluginStartup)Activator.CreateInstance(type)!)
            .ToArray();

        foreach (var startup in startupImplementations)
        {
            startup.Configure(app, configuration);
        }

        return app;
    }

    public static IConfiguration BuildPluginConfiguration(this PluginContext pluginContext, IConfiguration rootConfiguration, string environmentName)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddConfiguration(rootConfiguration)
            .SetBasePath(pluginContext.PluginFolder)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        return configurationBuilder.Build();
    }
}
