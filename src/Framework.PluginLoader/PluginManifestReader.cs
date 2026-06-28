namespace Framework.PluginLoader;

public static class PluginManifestReader
{
    public static Framework.Abstractions.PluginManifest Read(string manifestPath)
    {
        var json = File.ReadAllText(manifestPath);
        var manifest = JsonSerializer.Deserialize<Framework.Abstractions.PluginManifest>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        });

        return manifest ?? throw new InvalidOperationException($"Unable to read plugin manifest from '{manifestPath}'.");
    }
}
