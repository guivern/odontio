using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Commands.AddPatientDiseases;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class AddPatientDiseasesCommand: IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
    public List<long> DiseaseIds { get; set; } = new();
}