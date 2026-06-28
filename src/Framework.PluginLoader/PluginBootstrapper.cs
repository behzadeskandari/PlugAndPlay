using Microsoft.AspNetCore.Builder;

namespace Framework.PluginLoader;

public static class PluginBootstrapper
{
    public static void AddPluginServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PluginOptions>(configuration.GetSection("Plugins"));
        services.AddSingleton<PluginOptions>(sp => sp.GetRequiredService<IOptions<PluginOptions>>().Value);
        services.AddSingleton<PluginCatalog>();
        services.AddSingleton<PluginRegistry>();
        services.AddSingleton<PluginDependencyResolver>();
        services.AddSingleton<PluginLoader>();
        services.AddSingleton<IPluginResolver, PluginResolver>();
    }

    public static IReadOnlyCollection<PluginContext> LoadPlugins(string rootPath, IServiceProvider serviceProvider)
    {
        var loader = serviceProvider.GetRequiredService<PluginLoader>();
        return loader.LoadPlugins(rootPath);
    }
}
