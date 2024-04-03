using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;

namespace Odontio.Application.Diseases.Queries.GetDiseases;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetDiseasesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<UpsertDiseaseResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
}