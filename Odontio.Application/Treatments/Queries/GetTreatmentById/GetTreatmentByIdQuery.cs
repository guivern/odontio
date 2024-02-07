using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Queries.GetTreatmentById;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.User), nameof(RolesEnum.Administrator))]
public class GetTreatmentByIdQuery: PagedListQueryBase, IRequest<ErrorOr<GetTreatmentByIdResult>>, IWorkspaceResource
{
    [FromRoute] public long Id { get; init; }
    [FromRoute] public long WorkspaceId { get; init; }
}