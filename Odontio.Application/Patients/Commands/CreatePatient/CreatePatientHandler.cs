using Odontio.Application.Common.Interfaces;
using Odontio.Application.Patients.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Patients.Commands.CreatePatient;

public class CreatePatientHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<CreatePatientCommand, ErrorOr<UpsertPatientResult>>
{
    public async Task<ErrorOr<UpsertPatientResult>> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var patient = mapper.Map<Patient>(command);
        
        context.Patients.Add(patient);
        
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<UpsertPatientResult>(patient);
        
        return result;
    }
}