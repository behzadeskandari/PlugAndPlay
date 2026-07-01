using Mapster;

namespace Framework.Application.Mapping;

public interface IMapProfile
{
    void Register(TypeAdapterConfig config);
}
