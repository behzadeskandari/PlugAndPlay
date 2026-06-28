namespace Framework.PluginLoader;

public sealed class PluginContext
{
    public Framework.Abstractions.PluginManifest Manifest { get; init; } = default!;
    public string PluginFolder { get; init; } = default!;
    public PluginAssemblyLoader AssemblyLoader { get; init; } = default!;
    public Assembly PluginAssembly { get; init; } = default!;
    public PluginLifecycle Lifecycle { get; set; } = PluginLifecycle.Enabled;
}
