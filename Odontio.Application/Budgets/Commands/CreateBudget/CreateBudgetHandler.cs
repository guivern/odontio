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
        
        var addTreatmentsResult = await AddPatientTreatments(budget, request.Details, cancellationToken);
        if (addTreatmentsResult.IsError)
        {
            return addTreatmentsResult.Errors;
        }
        
        context.Budgets.Add(budget);
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<UpsertBudgetResult>(budget);
        
        return result;
    }
    
    private async Task<ErrorOr<Success>> AddPatientTreatments(Budget budget, IEnumerable<CreatePatientTreatment> details, CancellationToken cancellationToken)
    {
        budget.PatientTreatments.Clear();
        foreach (var budgetDetail in details)
        {
            var addTreatmentResult = await AddPatientTreatment(budget, budgetDetail, cancellationToken);
            if (addTreatmentResult.IsError)
            {
                return addTreatmentResult.Errors;
            }
        }

        return Result.Success;
    }

    private async Task<ErrorOr<Success>> AddPatientTreatment(Budget budget, CreatePatientTreatment budgetDetail, CancellationToken cancellationToken)
    {
        PatientTreatment patientTreatment = new()
        {
            TreatmentId = budgetDetail.Treatment.Id,
            Observations = budgetDetail.Observations,
            Cost = budgetDetail.Cost
        };

        switch (budgetDetail.Diagnosis)
        {
            case { Id: not null }:
                var existingDiagnosis = await context.Diagnoses.FindAsync(new object[] { budgetDetail.Diagnosis.Id }, cancellationToken);
                if (existingDiagnosis is null)
                {
                    return Error.NotFound(nameof(Diagnosis), "Diagnosis Not Found");
                }
                existingDiagnosis.Description = budgetDetail.Diagnosis.Description;
                existingDiagnosis.Observations = budgetDetail.Diagnosis.Observations;
                patientTreatment.DiagnosisId = budgetDetail.Diagnosis.Id;
                break;

            case not null:
                var newDiagnosis = mapper.Map<Diagnosis>(budgetDetail.Diagnosis);
                newDiagnosis.PatientId = budget.PatientId;
                patientTreatment.Diagnosis = newDiagnosis;
                break;
        }

        budget.PatientTreatments.Add(patientTreatment);
        return Result.Success;
    }
}