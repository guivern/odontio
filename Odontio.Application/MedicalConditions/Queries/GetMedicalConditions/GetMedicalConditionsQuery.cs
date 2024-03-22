using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.MedicalConditions.Queries.GetMedicalConditions;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(Roles.Administrator))]
public class GetMedicalConditionsQuery: IRequest<ErrorOr<List<MedicalConditionResult>>>, IPatientResource
{
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}