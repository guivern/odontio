using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.UpdateBudget;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class UpdateBudgetCommand : IRequest<ErrorOr<UpsertBudgetResult>>, IPatientResource
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    
    public List<UpdatePatientTreatment> PatientTreatments { get; set; } = new();
}

public class UpdatePatientTreatment
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}