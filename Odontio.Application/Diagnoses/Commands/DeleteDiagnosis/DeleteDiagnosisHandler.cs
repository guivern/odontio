using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Diagnoses.Commands.DeleteDiagnosis;

public class DeleteDiagnosisHandler(IApplicationDbContext context): IRequestHandler<DeleteDiagnosisCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteDiagnosisCommand request, CancellationToken cancellationToken)
    {
        var diagnosis = await context.Diagnoses
            .Where(x => x.Id == request.Id && x.PatientId == request.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (diagnosis == null)
        {
            return Error.NotFound(description: "Diagnosis not found");
        }

        context.Diagnoses.Remove(diagnosis);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            return Error.Conflict(description: "Can not delete diagnosis due to existing dependencies.");
        }

        return Unit.Value;
    }
}