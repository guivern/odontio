using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Queries.GetTreatmentById;

public class GetTreatmentByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetTreatmentByIdQuery, ErrorOr<GetTreatmentByIdResult>>
{
    public async Task<ErrorOr<GetTreatmentByIdResult>> Handle(GetTreatmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var treatment = context.Treatments
            .AsNoTracking()
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .FirstOrDefault(x => x.Id == request.Id);

        if (treatment == null)
        {
            return Error.NotFound(description: "Treatment not found");
        }

        var result = mapper.Map<GetTreatmentByIdResult>(treatment);
        
        return result;
    }
}