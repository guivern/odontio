using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Queries.GetPatientById;

public class GetPatientByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPatientByIdQuery, ErrorOr<GetPatientByIdResult>>
{
    public async Task<ErrorOr<GetPatientByIdResult>> Handle(GetPatientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var patient = await context.Patients
            .Include(p => p.Referred)
            .Where(p => p.Id == request.Id && p.WorkspaceId == request.WorkspaceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (patient == null)
        {
            return Error.NotFound(description: "Patient not found");
        }

        var result = mapper.Map<GetPatientByIdResult>(patient);

        return result;
    }
}