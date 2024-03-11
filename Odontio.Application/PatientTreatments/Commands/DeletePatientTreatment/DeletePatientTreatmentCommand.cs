using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientTreatments.Commands.DeletePatientTreatment;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeletePatientTreatmentCommand : IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public long BudgetId { get; set; }
}