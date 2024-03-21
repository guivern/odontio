using Odontio.Domain.Entities;

namespace Odontio.Application.Users.Common;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<User, GetUs>()
    }
}