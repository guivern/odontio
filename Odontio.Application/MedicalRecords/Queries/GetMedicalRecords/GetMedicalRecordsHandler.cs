using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalRecords.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.MedicalRecords.Queries.GetMedicalRecords;

public class GetMedicalRecordsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMedicalRecordsQuery, ErrorOr<PagedList<GetMedicalRecordFullResult>>>
{
    public async Task<ErrorOr<PagedList<GetMedicalRecordFullResult>>> Handle(GetMedicalRecordsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.MedicalRecords
            .Include(x => x.Appointment)
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Budget)
            .ThenInclude(x => x.Patient)
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Treatment)
            .Where(x => x.PatientTreatment.Budget.Patient.WorkspaceId == request.WorkspaceId);

        if (request.PatientId != null)
        {
            var patientExits =
                await context.Patients.AnyAsync(x => x.Id == request.PatientId && x.WorkspaceId == request.WorkspaceId,
                    cancellationToken);

            if (!patientExits)
                return Error.NotFound(description: "Patient not found");

            query = query.Where(x => x.Appointment.PatientId == request.PatientId);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(MedicalRecord.PatientTreatment)}.{nameof(PatientTreatment.Treatment)}.{nameof(Treatment.Name)}",
                $"{nameof(MedicalRecord.PatientTreatment)}.{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.FirstName)}",
                $"{nameof(MedicalRecord.PatientTreatment)}.{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.LastName)}",
                $"{nameof(MedicalRecord.PatientTreatment)}.{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.DocumentNumber)}"
            });
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<MedicalRecord>.CreateAsync(query, request.Page, request.PageSize);
        
        var dto = mapper.Map<PagedList<GetMedicalRecordFullResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
    }
}