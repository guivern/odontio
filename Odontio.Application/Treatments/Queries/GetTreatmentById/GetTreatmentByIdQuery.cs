using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Treatments.Queries.GetTreatmentById;

[RolesAuthorize(nameof(RolesEnum.User), nameof(RolesEnum.Administrator))]
public class GetTreatmentByIdQuery: PagedListQueryBase, IRequest<ErrorOr<GetTreatmentByIdResult>>
{
    [FromRoute] public long Id { get; init; }
    [FromRoute] public long WorkspaceId { get; init; }
}