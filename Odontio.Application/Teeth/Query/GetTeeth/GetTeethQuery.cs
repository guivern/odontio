using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Teeth.Query.GetTeeth;

[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetTeethQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetToothResult>>>
{
    public string? Odontogram { get; set; }
}