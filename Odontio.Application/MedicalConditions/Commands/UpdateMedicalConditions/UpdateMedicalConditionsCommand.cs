using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.MedicalConditions.Common.UpdateMedicalConditions;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdateMedicalConditionsCommand : IRequest<ErrorOr<List<MedicalConditionResult>>>, IPatientResource
{
    public List<MedicalConditionDto> MedicalConditions { get; set; } = [];

    public long PatientId { get; set; }

    public long WorkspaceId { get; set; }
}

public class MedicalConditionDto
{
    public string ConditionType { get; set; } = null!;
    public bool? HasCondition { get; set; }
    public string? Description { get; set; }
}