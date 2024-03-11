using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientTreatments.Commands.DeletePatientTreatment;

public class DeletePatientTreatmentHandler(IApplicationDbContext context) : IRequestHandler<DeletePatientTreatmentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePatientTreatmentCommand request, CancellationToken cancellationToken)
    {
        var patientTreatment = await context.PatientTreatments
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (patientTreatment is null)
        {
            return Error.NotFound(description: "Patient treatment not found");
        }
        
        var budget = await context.Budgets
            .Where(x => x.Id == request.BudgetId && x.PatientId == request.PatientId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (budget is null)
        {
            return Error.NotFound(description: "Budget not found");
        }
        
        if (budget.PatientTreatments.Count == 1)
        {
            return Error.Validation(description: "A budget must have at least one treatment");
        }

        context.PatientTreatments.Remove(patientTreatment);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}