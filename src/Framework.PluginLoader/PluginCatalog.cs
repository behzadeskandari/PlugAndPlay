namespace Framework.PluginLoader;

public sealed class PluginCatalog
{
    private readonly PluginOptions _options;
    private readonly ILogger<PluginCatalog> _logger;

    public PluginCatalog(PluginOptions options, ILogger<PluginCatalog> logger)
    {
        _options = options;
        _logger = logger;
    }

    public IReadOnlyCollection<string> DiscoverPluginFolders(string rootPath)
    {
        var folder = Path.GetFullPath(Path.Combine(rootPath, _options.PluginsPath));
        if (!Directory.Exists(folder))
        {
            _logger.LogWarning("Plugins folder does not exist: {PluginsPath}", folder);
            return Array.Empty<string>();
        }

        return Directory.EnumerateDirectories(folder)
            .ToArray();
    }
}
