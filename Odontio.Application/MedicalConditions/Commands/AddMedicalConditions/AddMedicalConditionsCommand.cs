using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;

namespace Odontio.Application.MedicalConditions.Common.AddMedicalConditions;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class AddMedicalConditionsCommand : IRequest<ErrorOr<List<MedicalConditionResult>>>, IPatientResource
{
    public List<AddMedicalConditionDto> MedicalConditions { get; set; } = [];

    public long PatientId { get; set; }

    public long WorkspaceId { get; set; }
}

public class AddMedicalConditionDto
{
    public string ConditionType { get; set; } = null!;
    public bool HasCondition { get; set; }
    public string? Description { get; set; }
}