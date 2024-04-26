using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Treatments.Queries.GetTreatments;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetTreatmentsQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetTreatmentsResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; init; }
}