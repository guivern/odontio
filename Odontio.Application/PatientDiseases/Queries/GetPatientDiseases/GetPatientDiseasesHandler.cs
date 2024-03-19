using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Queries.GetPatientDiseases;

public class GetPatientDiseasesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPatientDiseasesQuery, List<GetPatientDiseaseResult>>
{
    public async Task<List<GetPatientDiseaseResult>> Handle(GetPatientDiseasesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await context.PatientDiseases
            .AsNoTracking()
            .Where(x => x.PatientId == request.PatientId)
            .ProjectToType<GetPatientDiseaseResult>()
            .ToListAsync(cancellationToken);

        return result;
    }
}