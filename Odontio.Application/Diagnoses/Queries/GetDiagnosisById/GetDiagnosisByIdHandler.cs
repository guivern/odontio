using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;

namespace Odontio.Application.Diagnoses.Queries.GetDiagnosisById;

public class GetDiagnosisByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetDiagnosisByIdQuery, ErrorOr<UpsertDiagnosisResult>>
{
    public async Task<ErrorOr<UpsertDiagnosisResult>> Handle(GetDiagnosisByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await context.Diagnoses
            .Include(x => x.Tooth)
            .Where(x => x.Id == request.Id && x.PatientId == request.PatientId)
            .ProjectToType<UpsertDiagnosisResult>()
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return Error.NotFound(description: "Diagnosis not found");
        }
        
        return result;
    }
}