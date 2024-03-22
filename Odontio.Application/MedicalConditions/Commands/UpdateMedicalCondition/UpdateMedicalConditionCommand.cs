using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.MedicalConditions.UpdateMedicalCondition;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdateMedicalConditionCommand: IRequest<ErrorOr<MedicalConditionResult>>, IPatientResource
{
    public long Id { get; set; }
    public string ConditionType { get; set; } = null!;
    public bool? HasCondition { get; set; }
    public string? Description { get; set; }
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}
