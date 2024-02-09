using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Patients.Commands.CreatePatient;

public class CreatePatientHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<CreatePatientCommand, ErrorOr<CreatePatientResult>>
{
    public async Task<ErrorOr<CreatePatientResult>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = mapper.Map<Patient>(request);
        
        context.Patients.Add(patient);
        
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<CreatePatientResult>(patient);
        
        return result;
    }
}