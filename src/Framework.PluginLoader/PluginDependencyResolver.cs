namespace Framework.PluginLoader;

public sealed class PluginDependencyResolver
{
    public IReadOnlyCollection<PluginContext> Sort(IEnumerable<PluginContext> plugins)
    {
        var dict = plugins.ToDictionary(plugin => plugin.Manifest.Id, StringComparer.OrdinalIgnoreCase);
        var visited = new Dictionary<string, VisitState>(StringComparer.OrdinalIgnoreCase);
        var result = new List<PluginContext>();

        foreach (var plugin in plugins)
        {
            Visit(plugin, dict, visited, result);
        }

        return result;
    }

    private static void Visit(PluginContext plugin, Dictionary<string, PluginContext> dict, Dictionary<string, VisitState> visited, List<PluginContext> result)
    {
        if (visited.TryGetValue(plugin.Manifest.Id, out var state))
        {
            if (state == VisitState.Visiting)
            {
                throw new InvalidOperationException($"Circular plugin dependency detected for plugin '{plugin.Manifest.Id}'.");
            }

            return;
        }

        visited[plugin.Manifest.Id] = VisitState.Visiting;

        foreach (var dependency in plugin.Manifest.Dependencies)
        {
            if (dict.TryGetValue(dependency, out var dependencyPlugin))
            {
                Visit(dependencyPlugin, dict, visited, result);
            }
        }

        visited[plugin.Manifest.Id] = VisitState.Visited;
        result.Add(plugin);
    }

    private enum VisitState
    {
        NotVisited,
        Visiting,
        Visited
    }
}
