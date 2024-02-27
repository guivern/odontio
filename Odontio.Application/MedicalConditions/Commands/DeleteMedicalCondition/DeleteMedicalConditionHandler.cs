using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalConditions.Common.DeleteMedicalCondition;

public class DeleteMedicalConditionHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteMedicalConditionCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var medicalCondition = await context.MedicalConditions
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.PatientId == request.PatientId, cancellationToken);

        if (medicalCondition == null)
        {
            return Error.NotFound(description: "Medical condition not found");
        }

        if (medicalCondition.PatientId != request.PatientId)
        {
            return Error.Validation(description: "Medical condition does not belong to patient");
        }

        context.MedicalConditions.Remove(medicalCondition);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}