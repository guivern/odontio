using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Treatments.Queries.GetTreatments;

[RolesAuthorize(nameof(RolesEnum.User), nameof(RolesEnum.Administrator))]
public class GetTreatmentsQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetTreatmentsResult>>>
{
    [FromRoute] public long WorkspaceId { get; init; }
}