using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Commands.DeletePatientDisease;

public class DeletePatientDiseaseHandler(IApplicationDbContext context)
    : IRequestHandler<DeletePatientDiseaseCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePatientDiseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.PatientDiseases
            .FindAsync(request.Id);

        if (entity == null)
        {
            return Error.NotFound(description: "Patient disease not found");
        }
        
        if (entity.PatientId != request.PatientId)
        {
            return Error.Validation(description: "Patient disease does not belong to patient");
        }

        context.PatientDiseases.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}