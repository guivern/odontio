using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.PatientDiseases.Commands.DeletePatientDisease;

public class DeletePatientDiseaseHandler(IApplicationDbContext context)
    : IRequestHandler<DeletePatientDiseaseCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePatientDiseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.PatientDiseases
            .Include(x => x.Patient)
            .Where(x => x.Patient.Id == request.PatientId)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

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