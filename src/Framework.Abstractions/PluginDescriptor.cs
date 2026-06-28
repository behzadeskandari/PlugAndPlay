namespace Framework.Abstractions;

public sealed class PluginDescriptor
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Version { get; init; }
    public required string Author { get; init; }
    public required string Assembly { get; init; }
    public required string MinimumHostVersion { get; init; }
    public required string Description { get; init; }
    public IReadOnlyList<string> Dependencies { get; init; } = Array.Empty<string>();
}
