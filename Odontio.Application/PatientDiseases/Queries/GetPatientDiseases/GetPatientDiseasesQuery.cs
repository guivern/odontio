using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.PatientDiseases.Queries.GetPatientDiseases;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(Roles.Administrator))]
public class GetPatientDiseasesQuery: IRequest<ErrorOr<List<GetPatientDiseaseResult>>>, IPatientResource
{
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}
