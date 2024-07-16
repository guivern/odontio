using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.UpdateBudget;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdateBudgetCommand : IRequest<ErrorOr<UpsertBudgetResult>>, IPatientResource
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string? Observations { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    
    public List<UpdatePatientTreatment> PatientTreatments { get; set; } = new();
}

public class UpdatePatientTreatment
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public long? DiagnosisId { get; set; }
    public decimal Cost { get; set; }
}