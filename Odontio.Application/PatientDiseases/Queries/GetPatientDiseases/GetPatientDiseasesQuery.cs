using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Queries.GetPatientDiseases;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetPatientDiseasesQuery: IRequest<List<GetPatientDiseaseResult>>, IPatientResource
{
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}
