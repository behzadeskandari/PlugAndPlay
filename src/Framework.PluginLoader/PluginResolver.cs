namespace Framework.PluginLoader;

public sealed class PluginResolver : IPluginResolver
{
    public IEnumerable<T> ResolveServices<T>(PluginContext pluginContext)
    {
        return pluginContext.PluginAssembly.GetTypes()
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(type => (T)Activator.CreateInstance(type)!)
            .ToArray();
    }

    public IEnumerable<object> ResolveServices(PluginContext pluginContext, Type serviceType)
    {
        return pluginContext.PluginAssembly.GetTypes()
            .Where(type => serviceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(type => Activator.CreateInstance(type)!)
            .ToArray();
    }
}
