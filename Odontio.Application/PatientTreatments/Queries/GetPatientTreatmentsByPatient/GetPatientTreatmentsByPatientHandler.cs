using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatmentsByPatient;

public class GetPatientTreatmentsByPatientHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPatientTreatmentsByPatientQuery, PagedList<GetPatientTreatmentFullResult>>
{
    public async Task<PagedList<GetPatientTreatmentFullResult>> Handle(GetPatientTreatmentsByPatientQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.PatientTreatments
            .Include(x => x.Budget)
            .Include(x => x.Treatment)
            .Include(x => x.Tooth)
            .Include(x => x.MedicalRecords)
            .Where(x => x.Budget.PatientId == request.PatientId)
            .ProjectToType<GetPatientTreatmentFullResult>()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(PatientTreatment.Treatment)}.{nameof(Treatment.Name)}",
                $"{nameof(PatientTreatment.Tooth)}.{nameof(Tooth.Name)}"
            });
        }
        
        if (request.Status != null)
        {
            query = query.Where(x => GetStatusFromString(x.Status) == request.Status);
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        return await PagedList<GetPatientTreatmentFullResult>.CreateAsync(query, request.Page, request.PageSize);
    }

    private TreatmentStatus? GetStatusFromString(string xStatus)
    {
        return xStatus switch
        {
            "Pending" => TreatmentStatus.Pending,
            "InProgress" => TreatmentStatus.InProgress,
            "Finished" => TreatmentStatus.Finished,
            _ => null
        };
    }
}