namespace Framework.Abstractions;

public interface IPluginInstaller
{
    Task<Result> InstallAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
    Task<Result> UninstallAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}

public interface IPluginPermissionProvider
{
    IEnumerable<string> GetPermissions();
}

public interface IPluginMenuProvider
{
    IEnumerable<PluginMenuItem> GetMenuItems();
}

public interface IPluginBackgroundJob
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public interface IPluginConfiguration
{
    void ConfigurePluginOptions(IServiceCollection services, IConfiguration configuration);
}

public interface IPluginSeeder
{
    Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}

public interface IPluginEventHandler<TEvent> where TEvent : PluginEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
