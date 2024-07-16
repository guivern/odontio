using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalNotes.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.MedicalNotes.Queries.GetMedicalNotes;

public class GetMedicalNotesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMedicalNotesQuery, ErrorOr<PagedList<GetMedicalNoteFullResult>>>
{
    public async Task<ErrorOr<PagedList<GetMedicalNoteFullResult>>> Handle(GetMedicalNotesQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.MedicalNotes
            .AsNoTracking()
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
                $"{nameof(Domain.Entities.MedicalNote.PatientTreatment)}.{nameof(PatientTreatment.Treatment)}.{nameof(Treatment.Name)}",
                $"{nameof(Domain.Entities.MedicalNote.PatientTreatment)}.{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.FirstName)}",
                $"{nameof(Domain.Entities.MedicalNote.PatientTreatment)}.{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.LastName)}",
                $"{nameof(Domain.Entities.MedicalNote.PatientTreatment)}.{nameof(PatientTreatment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.DocumentNumber)}"
            });
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<Domain.Entities.MedicalNote>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);
        
        var dto = mapper.Map<PagedList<GetMedicalNoteFullResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
    }
}