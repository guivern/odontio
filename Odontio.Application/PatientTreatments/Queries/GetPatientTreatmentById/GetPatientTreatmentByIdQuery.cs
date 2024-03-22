using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatmentById;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetPatientTreatmentByIdQuery : IRequest<ErrorOr<GetPatientTreatmentFullResult>>, IPatientResource
{
    public long Id { get; set; }
    public long BudgetId { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}