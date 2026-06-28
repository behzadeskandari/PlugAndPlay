namespace Framework.PluginLoader;

public interface IPluginResolver
{
    IEnumerable<T> ResolveServices<T>(PluginContext pluginContext);
    IEnumerable<object> ResolveServices(PluginContext pluginContext, Type serviceType);
}
