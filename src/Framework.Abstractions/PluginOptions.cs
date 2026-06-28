namespace Framework.Abstractions;

public sealed class PluginOptions
{
    public string PluginsPath { get; set; } = "plugins";
    public bool EnablePluginIsolation { get; set; } = true;
    public string PluginManifestFileName { get; set; } = "plugin.json";
}
