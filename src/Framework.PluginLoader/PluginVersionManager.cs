namespace Framework.PluginLoader;

public sealed class PluginVersionManager
{
    public bool IsCompatible(Framework.Abstractions.PluginManifest manifest, string hostVersion)
    {
        if (Version.TryParse(hostVersion, out var host) && Version.TryParse(manifest.MinimumHostVersion, out var minimum))
        {
            return host >= minimum;
        }

        return false;
    }
}
