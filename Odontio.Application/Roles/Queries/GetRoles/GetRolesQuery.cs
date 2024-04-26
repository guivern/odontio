using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Roles.Queries.GetRoles;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetRolesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetRolesResult>>>
{
}