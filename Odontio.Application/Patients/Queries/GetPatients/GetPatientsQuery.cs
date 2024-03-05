using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Queries.GetPatients;

[ValidateWorkspace]
public class GetPatientsQuery : PagedListQueryBase, IRequest<PagedList<GetPatientsResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
}