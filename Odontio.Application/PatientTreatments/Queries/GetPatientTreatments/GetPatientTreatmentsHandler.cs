using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatments;

public class GetPatientTreatmentsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPatientTreatmentsQuery, ErrorOr<PagedList<GetPatientTreatmentFullResult>>>
{
    public async Task<ErrorOr<PagedList<GetPatientTreatmentFullResult>>> Handle(GetPatientTreatmentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.PatientTreatments
            .Include(x => x.Budget)
            .ThenInclude(x => x.Patient)
            .Include(x => x.Treatment)
            .Include(x => x.Tooth)
            .Include(x => x.MedicalRecords)
            .Where(x => x.Budget.Patient.WorkspaceId == request.WorkspaceId)
            .AsQueryable();
        
        if (request.PatientId != null)
        {
            var patientExits = await context.Patients.AnyAsync(x => x.Id == request.PatientId
                && x.WorkspaceId == request.WorkspaceId, cancellationToken);
            
            if (!patientExits)
                return Error.NotFound(description: "Patient not found");
            
            query = query.Where(x => x.Budget.Patient.Id == request.PatientId);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(PatientTreatment.Treatment)}.{nameof(Treatment.Name)}",
                $"{nameof(PatientTreatment.Tooth)}.{nameof(Tooth.Name)}",
                $"{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.FirstName)}",
                $"{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.LastName)}",
                $"{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.DocumentNumber)}"
            });
        }
        
        // TODO VALIDAR SI ESTO FUNCIONA
        // if (request.Status != null)
        // {
        //     query = query.Where(x => GetStatusFromString(x.Status) == request.Status);
        // }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<PatientTreatment>.CreateAsync(query, request.Page, request.PageSize);
        
        var dto = mapper.Map<PagedList<GetPatientTreatmentFullResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
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
