using Odontio.Application.Profile.Queries.GetProfile;
using Odontio.Domain.Entities;

namespace Odontio.Application.Profile.Common;

public class MappingConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetProfileResult>()
            .Map(dest => dest.RoleName, src => src.Role.Name)
            .Map(dest => dest.WorkspaceName, src => src.Workspace.Name);
    }
}