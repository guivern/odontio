using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Treatments.Common.CreateTreatment;

public class CreateTreatmentHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateTreatmentCommand, ErrorOr<CreateTreatmentResult>>
{
    public async Task<ErrorOr<CreateTreatmentResult>> Handle(CreateTreatmentCommand request,
        CancellationToken cancellationToken)
    {
        var workspaceExists = await context.Workspaces.AsNoTracking()
            .AnyAsync(x => x.Id == request.WorkspaceId, cancellationToken);
        
        if (!workspaceExists)
            return Error.NotFound("Workspace does not exist");
        
        var treatment = mapper.Map<Treatment>(request);
        
        context.Treatments.Add(treatment);
        
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<CreateTreatmentResult>(treatment);
        
        return result;
    }
}