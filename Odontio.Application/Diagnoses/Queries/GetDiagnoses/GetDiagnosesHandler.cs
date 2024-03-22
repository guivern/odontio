using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;

namespace Odontio.Application.Diagnoses.Queries.GetDiagnoses;

public class GetDiagnosesHandler(IApplicationDbContext context): IRequestHandler<GetDiagnosesQuery, ErrorOr<List<UpsertDiagnosisResult>>>
{
    public async Task<ErrorOr<List<UpsertDiagnosisResult>>> Handle(GetDiagnosesQuery request, CancellationToken cancellationToken)
    {
        var diagnoses = await context.Diagnoses
            .AsNoTracking()
            .Where(x => x.PatientId == request.PatientId)
            .ProjectToType<UpsertDiagnosisResult>()
            .ToListAsync(cancellationToken);

        return diagnoses;
    }
}