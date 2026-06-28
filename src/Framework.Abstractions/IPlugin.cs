namespace Framework.Abstractions;

public interface IPlugin
{
    PluginDescriptor Descriptor { get; }
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    void Configure(WebApplication app, IConfiguration configuration);
}
