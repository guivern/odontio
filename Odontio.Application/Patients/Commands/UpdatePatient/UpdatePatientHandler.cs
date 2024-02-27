using Odontio.Application.Common.Interfaces;
using Odontio.Application.Patients.Common;

namespace Odontio.Application.Patients.Commands.UpdatePatient;

public class UpdatePatientHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdatePatientCommand, ErrorOr<UpsertPatientResult>>
{
    public async Task<ErrorOr<UpsertPatientResult>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await context.Patients.FindAsync(request.Id);
        
        if (patient == null)
        {
            return Error.NotFound(description: "Patient not found");
        }
        
        patient = mapper.Map(request, patient);
        
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The patient was modified by another user");
        }
        
        var result = mapper.Map<UpsertPatientResult>(patient);
        
        return result;
    }
}