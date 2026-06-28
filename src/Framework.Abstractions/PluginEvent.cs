namespace Framework.Abstractions;

public abstract class PluginEvent
{
    public required string PluginId { get; init; }
    public required DateTime OccurredAt { get; init; }
}
