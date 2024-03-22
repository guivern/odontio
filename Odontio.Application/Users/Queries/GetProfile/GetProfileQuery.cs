using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Queries.GetProfile;

[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.Doctor), nameof(RolesEnum.Assistant))]
public class GetProfileQuery : IRequest<ErrorOr<GetProfileResult>>
{
    public long Id { get; set; }
}