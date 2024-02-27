using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;

namespace Odontio.Application.MedicalConditions.Queries.GetMedicalConditions;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetMedicalConditionsQuery: IRequest<ErrorOr<IEnumerable<MedicalConditionResult>>>, IPatientResource
{
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}