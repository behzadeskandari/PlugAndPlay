using Framework.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Identity.Plugin.Startup;

public sealed class IdentityPluginStartup : IPluginStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register plugin services, DbContexts, repositories, MediatR handlers, etc.
    }

    public void Configure(IApplicationBuilder app, IConfiguration configuration)
    {
        // Configure middleware, endpoints specific to identity plugin
    }
}
