namespace Framework.PluginLoader;

public sealed class PluginRegistry
{
    private readonly Dictionary<string, PluginContext> _plugins = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyCollection<PluginContext> Plugins => _plugins.Values;

    public void Register(PluginContext context)
    {
        _plugins[context.Manifest.Id] = context;
    }

    public bool TryGet(string pluginId, out PluginContext? context)
        => _plugins.TryGetValue(pluginId, out context);
}
