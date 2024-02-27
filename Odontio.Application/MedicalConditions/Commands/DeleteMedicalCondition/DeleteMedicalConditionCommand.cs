using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalConditions.Common.DeleteMedicalCondition;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeleteMedicalConditionCommand : IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}