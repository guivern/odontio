using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Treatments.Commands.CreateTreatment;

public class CreateTreatmentHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateTreatmentCommand, ErrorOr<CreateTreatmentResult>>
{
    public async Task<ErrorOr<CreateTreatmentResult>> Handle(CreateTreatmentCommand request,
        CancellationToken cancellationToken)
    {
        var treatment = mapper.Map<Treatment>(request);
        
        context.Treatments.Add(treatment);
        
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<CreateTreatmentResult>(treatment);
        
        return result;
    }
}