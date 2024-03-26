using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientTreatments.Commands.DeletePatientTreatment;

public class DeletePatientTreatmentHandler(IApplicationDbContext context)
    : IRequestHandler<DeletePatientTreatmentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePatientTreatmentCommand request, CancellationToken cancellationToken)
    {
        var patientTreatment = await context.PatientTreatments
            .Include(x => x.Budget)
            .Where(x => x.Id == request.Id)
            .Where(x => x.BudgetId == request.BudgetId && x.Budget.PatientId == request.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (patientTreatment is null)
        {
            return Error.NotFound(description: "Patient treatment not found");
        }
        
        var patientTreatmentsCount = await context.PatientTreatments
            .AsNoTracking()
            .Where(x => x.BudgetId == request.BudgetId)
            .CountAsync(cancellationToken);

        if (patientTreatmentsCount == 1)
        {
            return Error.Validation(description: "A budget must have at least one treatment");
        }

        context.PatientTreatments.Remove(patientTreatment);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            return Error.Conflict(description: "Can not delete patient treatment due to existing dependencies.");
        }

        return Unit.Value;
    }
}