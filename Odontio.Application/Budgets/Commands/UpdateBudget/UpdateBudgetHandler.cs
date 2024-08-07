using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Budgets.Commands.UpdateBudget;

public class UpdateBudgetHandler : IRequestHandler<UpdateBudgetCommand, ErrorOr<UpsertBudgetResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateBudgetHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UpsertBudgetResult>> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _context.Budgets
            .Include(x => x.PatientTreatments)
            .FirstOrDefaultAsync(x => x.PatientId == request.PatientId && x.Id == request.Id, cancellationToken);

        if (budget is null)
        {
            return Error.NotFound(nameof(Budget), "Budget not found");
        }

        budget = _mapper.Map(request, budget);
        
        var addTreatmentsResult = await AddPatientTreatments(budget, request.Details, cancellationToken);
        if (addTreatmentsResult.IsError)
        {
            return addTreatmentsResult.Errors;
        }

        _context.Budgets.Entry(budget).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The Budget was modified by another user");
        }

        return _mapper.Map<UpsertBudgetResult>(budget);
    }

    private async Task<ErrorOr<Success>> AddPatientTreatments(Budget budget, IEnumerable<UpdatePatientTreatment> details, CancellationToken cancellationToken)
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

    private async Task<ErrorOr<Success>> AddPatientTreatment(Budget budget, UpdatePatientTreatment budgetDetail, CancellationToken cancellationToken)
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
                var existingDiagnosis = await _context.Diagnoses.FindAsync(new object[] { budgetDetail.Diagnosis.Id }, cancellationToken);
                if (existingDiagnosis is null)
                {
                    return Error.NotFound(nameof(Diagnosis), "Diagnosis Not Found");
                }
                existingDiagnosis.Description = budgetDetail.Diagnosis.Description;
                existingDiagnosis.Observations = budgetDetail.Diagnosis.Observations;
                patientTreatment.DiagnosisId = budgetDetail.Diagnosis.Id;
                break;

            case not null:
                var newDiagnosis = _mapper.Map<Diagnosis>(budgetDetail.Diagnosis);
                newDiagnosis.PatientId = budget.PatientId;
                patientTreatment.Diagnosis = newDiagnosis;
                break;
        }

        budget.PatientTreatments.Add(patientTreatment);
        return Result.Success;
    }
}