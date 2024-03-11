using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;

namespace Odontio.Application.PatientTreatments.Commands.CreatePatientTreatment;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class CreatePatientTreatmentCommand : IRequest<ErrorOr<UpsertPatientTreatmentResult>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public long BudgetId { get; set; }
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}