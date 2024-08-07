using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Teeth.Query.Common;

namespace Odontio.Application.Teeth.Query.GetTeeth;

[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetTeethQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetToothResult>>>
{
    public string? Odontogram { get; set; }
}