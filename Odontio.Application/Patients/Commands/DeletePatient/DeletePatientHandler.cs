using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Commands.DeletePatient;

public class DeletePatientHandler(IApplicationDbContext context) : IRequestHandler<DeletePatientCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await context.Patients.FindAsync(request.Id);

        if (patient == null)
        {
            return Error.NotFound(description: "Patient not found");
        }

        context.Patients.Remove(patient);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}