namespace Framework.Abstractions;

public interface IPluginEndpoint
{
    void MapEndpoints(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder endpoints);
}
