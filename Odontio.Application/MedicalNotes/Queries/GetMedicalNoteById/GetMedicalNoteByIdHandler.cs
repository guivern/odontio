using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalNotes.Common;

namespace Odontio.Application.MedicalNotes.Queries.GetMedicalNoteById;

public class GetMedicalNoteByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMedicalNoteByIdQuery, ErrorOr<GetMedicalNoteFullResult>>
{
    public async Task<ErrorOr<GetMedicalNoteFullResult>> Handle(GetMedicalNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var medicalNote = await context.MedicalNotes
            .AsNoTracking()
            .Include(x => x.Appointment)
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Budget)
            .ThenInclude(x => x.Patient)
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Treatment)
            .Where(x => x.PatientTreatment.Budget.Patient.WorkspaceId == request.WorkspaceId)
            .Where(x => x.Appointment.PatientId == request.PatientId)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (medicalNote == null)
        {
            return Error.NotFound(description: "Medical record not found");
        }

        var result = mapper.Map<GetMedicalNoteFullResult>(medicalNote);

        return result;
    }
}