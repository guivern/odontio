using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.CreateBudget;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class CreateBudgetCommand: IRequest<ErrorOr<UpsertBudgetResult>>, IPatientResource
{
    public DateOnly? Date { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    
    public List<CreatePatientTreatment> PatientTreatments { get; set; } = new();
}

public class CreatePatientTreatment
{
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}