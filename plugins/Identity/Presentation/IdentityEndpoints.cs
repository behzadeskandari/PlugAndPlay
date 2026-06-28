using Framework.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Identity.Plugin.Presentation;

public sealed class IdentityEndpoints : IPluginEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/plugins/identity/ping", () => new { plugin = "Identity", status = "pong" });
    }
}
