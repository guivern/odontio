using Mapster;

namespace Odontio.API.Common.Mapping;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Example:
        // config.NewConfig<AuthenticationResult, AuthenticationResponse>()
        //     .Map(dest => dest.Id, src => src.User.Id)
        //     .Map(dest => dest, src => src.User);
    }
}