namespace Framework.PluginLoader;

public sealed class PluginUninstaller
{
    private readonly ILogger<PluginUninstaller> _logger;

    public PluginUninstaller(ILogger<PluginUninstaller> logger)
    {
        _logger = logger;
    }

    public Task<Result> UninstallAsync(PluginContext pluginContext, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Uninstalling plugin {PluginId}", pluginContext.Manifest.Id);
        return Task.FromResult(Result.Ok());
    }
}
