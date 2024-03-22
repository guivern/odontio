using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Theeth.Query.GetTeeth;

[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetTheethQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetThoothResult>>>
{
    public string? Odontogram { get; set; }
}