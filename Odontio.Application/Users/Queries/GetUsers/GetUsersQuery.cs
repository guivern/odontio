using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Users.Queries.GetUsers;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetUsersQuery : PagedListQueryBase, IRequest<PagedList<GetUsersResult>>
{
}