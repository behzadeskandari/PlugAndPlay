namespace Framework.Abstractions;

public interface IPluginStartup
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, IConfiguration configuration);
}
