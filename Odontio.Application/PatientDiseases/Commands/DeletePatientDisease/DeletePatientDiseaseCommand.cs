using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Commands.DeletePatientDisease;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeletePatientDiseaseCommand : IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}