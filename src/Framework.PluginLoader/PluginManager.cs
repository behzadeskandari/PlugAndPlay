namespace Framework.PluginLoader;

public sealed class PluginManager
{
    private readonly PluginRegistry _registry;
    private readonly ILogger<PluginManager> _logger;

    public PluginManager(PluginRegistry registry, ILogger<PluginManager> logger)
    {
        _registry = registry;
        _logger = logger;
    }

    public IReadOnlyCollection<PluginContext> GetAllPlugins() => _registry.Plugins;

    public PluginContext? GetPlugin(string id)
    {
        _registry.TryGet(id, out var plugin);
        return plugin;
    }

    public void Enable(string id)
    {
        var plugin = GetPlugin(id);
        if (plugin is null)
        {
            _logger.LogWarning("Attempted to enable missing plugin {PluginId}", id);
            return;
        }

        plugin.Lifecycle = PluginLifecycle.Enabled;
        _logger.LogInformation("Plugin {PluginId} enabled", id);
    }

    public void Disable(string id)
    {
        var plugin = GetPlugin(id);
        if (plugin is null)
        {
            _logger.LogWarning("Attempted to disable missing plugin {PluginId}", id);
            return;
        }

        plugin.Lifecycle = PluginLifecycle.Disabled;
        _logger.LogInformation("Plugin {PluginId} disabled", id);
    }
}
