namespace Framework.PluginLoader;

public sealed class PluginLoader
{
    private readonly PluginOptions _options;
    private readonly ILogger<PluginLoader> _logger;
    private readonly PluginCatalog _catalog;
    private readonly PluginRegistry _registry;
    private readonly PluginDependencyResolver _dependencyResolver;

    public PluginLoader(PluginOptions options, ILogger<PluginLoader> logger, PluginCatalog catalog, PluginRegistry registry, PluginDependencyResolver dependencyResolver)
    {
        _options = options;
        _logger = logger;
        _catalog = catalog;
        _registry = registry;
        _dependencyResolver = dependencyResolver;
    }

    public IReadOnlyCollection<PluginContext> LoadPlugins(string rootPath)
    {
        var pluginFolders = _catalog.DiscoverPluginFolders(rootPath);
        foreach (var pluginFolder in pluginFolders)
        {
            try
            {
                var manifestFile = Path.Combine(pluginFolder, _options.PluginManifestFileName);
                if (!File.Exists(manifestFile))
                {
                    _logger.LogWarning("Skipping folder without manifest: {PluginFolder}", pluginFolder);
                    continue;
                }

                var manifest = PluginManifestReader.Read(manifestFile);
                var assemblyPath = FindPluginAssembly(pluginFolder, manifest.Assembly);
                if (assemblyPath is null)
                {
                    _logger.LogWarning("Plugin assembly not found for plugin {PluginId}: {AssemblyName}", manifest.Id, manifest.Assembly);
                    continue;
                }

                var loader = new PluginAssemblyLoader(assemblyPath);
                var assembly = loader.LoadFromAssemblyPath(assemblyPath);
                var context = new PluginContext
                {
                    Manifest = manifest,
                    PluginFolder = pluginFolder,
                    AssemblyLoader = loader,
                    PluginAssembly = assembly
                };

                _registry.Register(context);
                _logger.LogInformation("Loaded plugin {PluginId} from {PluginFolder}", manifest.Id, pluginFolder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading plugin from {PluginFolder}", pluginFolder);
            }
        }

        var ordered = _dependencyResolver.Sort(_registry.Plugins).ToList();
        return ordered;
    }

    private string? FindPluginAssembly(string pluginFolder, string assemblyName)
    {
        var candidate = Path.GetFullPath(Path.Combine(pluginFolder, assemblyName));
        if (File.Exists(candidate))
        {
            return candidate;
        }

        return Directory.EnumerateFiles(pluginFolder, assemblyName, SearchOption.AllDirectories).FirstOrDefault();
    }
}
