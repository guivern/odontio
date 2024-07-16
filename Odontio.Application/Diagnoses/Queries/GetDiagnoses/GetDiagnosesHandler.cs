using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Diagnoses.Queries.GetDiagnoses;

public class GetDiagnosesHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetDiagnosesQuery, 
    ErrorOr<PagedList<UpsertDiagnosisResult>>>
{
    public async Task<ErrorOr<PagedList<UpsertDiagnosisResult>>> Handle(GetDiagnosesQuery request, CancellationToken cancellationToken)
    {
        // var diagnoses = await context.Diagnoses
        //     .AsNoTracking()
        //     .Where(x => x.PatientId == request.PatientId)
        //     .ProjectToType<UpsertDiagnosisResult>()
        //     .ToListAsync(cancellationToken);
        //
        // return diagnoses;
        
           var query = context.Diagnoses
            .AsNoTracking()
            .Include(x => x.Patient)
            .Where(x => x.Patient.WorkspaceId == request.WorkspaceId)
            .AsQueryable();
        
        if (request.PatientId != null)
        {
            var patientExits = await context.Patients.AnyAsync(x => x.Id == request.PatientId
                && x.WorkspaceId == request.WorkspaceId, cancellationToken);
            
            if (!patientExits)
                return Error.NotFound(description: "Patient not found");
            
            query = query.Where(x => x.PatientId == request.PatientId);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(Diagnosis.Tooth)}.{nameof(Tooth.Name)}",
                $"{nameof(Diagnosis.Description)}",
            });
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<Diagnosis>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);
        
        var dto = mapper.Map<PagedList<UpsertDiagnosisResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
        
    }
}