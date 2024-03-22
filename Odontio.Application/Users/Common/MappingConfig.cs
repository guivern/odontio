using Odontio.Application.Users.Queries.GetUserById;
using Odontio.Application.Users.Queries.GetUsers;
using Odontio.Domain.Entities;

namespace Odontio.Application.Users.Common;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetUsersResult>()
            .Map(dest => dest.RoleName, src => src.Role.Name)
            .Map(dest => dest.WorkspaceName, src => src.Workspace.Name);
        
        config.NewConfig<User, GetUserByIdResult>()
            .Map(dest => dest.RoleName, src => src.Role.Name)
            .Map(dest => dest.WorkspaceName, src => src.Workspace.Name);
    }
}