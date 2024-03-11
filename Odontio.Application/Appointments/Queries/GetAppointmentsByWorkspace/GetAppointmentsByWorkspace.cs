using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Appointments.Queries.GetAppointmentsByWorkspace;

public class GetAppointmentsByWorkspace(IApplicationDbContext context) : IRequestHandler<GetAppointmentsByWorkspaceQuery, PagedList<GetAppointmentResult>>
{
    public async Task<PagedList<GetAppointmentResult>> Handle(GetAppointmentsByWorkspaceQuery request, CancellationToken cancellationToken)
    {
        var query = context.Appointments
            .Include(x => x.MedicalRecords)
            .ThenInclude(x => x.PatientTreatment)
            .ThenInclude(x => x.Treatment)
            .Include(x => x.Patient)
            .Where(x => x.Patient.WorkspaceId == request.WorkspaceId)
            .ProjectToType<GetAppointmentResult>();
        
        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Where(x => x.MedicalRecords.Any(mr => mr.TreatmentName.ToLower().Contains(request.Filter.ToLower())));
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(Appointment.Patient)}.{nameof(Patient.FirstName)}",
                $"{nameof(Appointment.Patient)}.{nameof(Patient.LastName)}",
                $"{nameof(Appointment.Patient)}.{nameof(Patient.DocumentNumber)}",
            });
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        return await PagedList<GetAppointmentResult>.CreateAsync(query, request.Page, request.PageSize);
    }
}