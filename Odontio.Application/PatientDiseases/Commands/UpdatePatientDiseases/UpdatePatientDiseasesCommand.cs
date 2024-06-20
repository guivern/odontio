using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Commands.UpdatePatientDiseases;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdatePatientDiseasesCommand: IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
    public List<long> DiseaseIds { get; set; } = new();
}