using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.Budgets.Commands.CreateBudget;

public class CreateBudgetHandler(IApplicationDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider): IRequestHandler<CreateBudgetCommand, ErrorOr<UpsertBudgetResult>>
{
    public async Task<ErrorOr<UpsertBudgetResult>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = mapper.Map<Budget>(request);
        
        budget.Date = request.Date ?? dateTimeProvider.Today;
        budget.Status = BudgetStatus.Pending;
        budget.ExpirationDate ??= budget.Date.AddMonths(1);

        foreach (var detail in request.Details)
        {
            if (detail.Diagnosis != null && detail.Diagnosis.Id != null)
            {
                var diagnosis = await context.Diagnoses.FindAsync(new object[] { detail.Diagnosis.Id }, cancellationToken);
                if (diagnosis == null)
                {
                    return Error.NotFound(nameof(Diagnosis), "Diagnosis Not Found");
                }
                
                budget.PatientTreatments.Add(new PatientTreatment
                {
                    TreatmentId = detail.Treatment.Id,
                    DiagnosisId = detail.Diagnosis.Id,
                    Observations = detail.Observations,
                    Cost = detail.Cost
                });      
            }
            else if (detail.Diagnosis != null)
            {
                var diagnosis = mapper.Map<Diagnosis>(detail.Diagnosis);
                budget.PatientTreatments.Add(new PatientTreatment
                {
                    TreatmentId = detail.Treatment.Id,
                    Diagnosis = diagnosis,
                    Observations = detail.Observations,
                    Cost = detail.Cost
                });
            }
        }
        
        context.Budgets.Add(budget);
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<UpsertBudgetResult>(budget);
        
        return result;
    }
}