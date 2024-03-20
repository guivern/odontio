using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Appointments.Queries.GetAppointments;

public class GetAppointmentsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetAppointmentsQuery, ErrorOr<PagedList<GetAppointmentResult>>>
{
    public async Task<ErrorOr<PagedList<GetAppointmentResult>>> Handle(GetAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.PatientId != null)
        {
            var patientExits = await context.Patients
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.PatientId && x.WorkspaceId == request.WorkspaceId, cancellationToken);

            if (!patientExits)
                return Error.NotFound(description: "Patient not found in this workspace");
        }

        var query = context.Appointments
            .AsNoTracking()
            .Include(x => x.Patient)
            .Where(x => x.Patient.WorkspaceId == request.WorkspaceId);

        if (request.PatientId != null)
        {
            query = query.Where(x => x.PatientId == request.PatientId);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            if (request.PatientId == null)
            {
                query = query.Filter(request.Filter, new List<string>
                {
                    $"{nameof(Appointment.Patient)}.{nameof(Patient.FirstName)}",
                    $"{nameof(Appointment.Patient)}.{nameof(Patient.LastName)}",
                    $"{nameof(Appointment.Patient)}.{nameof(Patient.DocumentNumber)}",
                });
            }
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<Appointment>.CreateAsync(query, request.Page, request.PageSize);
        var dto = mapper.Map<PagedList<GetAppointmentResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;

        return dto;
    }
}