using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Profile.Queries.GetProfile;

[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetProfileQuery : IRequest<ErrorOr<GetProfileResult>>
{
    public long Id { get; set; }
}