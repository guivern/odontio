using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;
using Odontio.Application.Patients.Queries.GetPatientById;

namespace Odontio.Application.Diseases.Queries.GetDiseaseById;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetDiseaseByIdQuery : IRequest<ErrorOr<UpsertDiseaseResult>>, IWorkspaceResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
}