using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Queries.GetPatients;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetPatientsQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetPatientsResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
}