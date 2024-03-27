using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalNotes.Common;

namespace Odontio.Application.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateMedicalNoteCommand, ErrorOr<UpsertMedicalNoteResult>>
{
    public async Task<ErrorOr<UpsertMedicalNoteResult>> Handle(UpdateMedicalNoteCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.MedicalNotes
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Budget)
            .Where(x => x.AppointmentId == request.AppointmentId)
            .Where(x => x.PatientTreatment.Budget.PatientId == request.PatientId)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        entity = mapper.Map(request, entity);

        context.MedicalNotes.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The medical record was modified by another user");
        }

        var result = mapper.Map<UpsertMedicalNoteResult>(entity);

        return result;
    }
}