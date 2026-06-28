namespace Framework.Abstractions;

public sealed class PluginMenuItem
{
    public required string Title { get; init; }
    public required string Url { get; init; }
    public string? Icon { get; init; }
}
