namespace Framework.PluginLoader;

public sealed class PluginInstaller
{
    private readonly ILogger<PluginInstaller> _logger;

    public PluginInstaller(ILogger<PluginInstaller> logger)
    {
        _logger = logger;
    }

    public Task<Result> InstallAsync(PluginContext pluginContext, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Installing plugin {PluginId}", pluginContext.Manifest.Id);
        return Task.FromResult(Result.Ok());
    }
}
