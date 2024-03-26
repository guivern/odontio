using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalRecords.Common;

namespace Odontio.Application.MedicalRecords.Commands.UpdateMedicalRecord;

public class UpdateMedicalRecordHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateMedicalRecordCommand, ErrorOr<UpsertMedicalRecordResult>>
{
    public async Task<ErrorOr<UpsertMedicalRecordResult>> Handle(UpdateMedicalRecordCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.MedicalRecords
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Budget)
            .Where(x => x.AppointmentId == request.AppointmentId)
            .Where(x => x.PatientTreatment.Budget.PatientId == request.PatientId)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        entity = mapper.Map(request, entity);

        context.MedicalRecords.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The medical record was modified by another user");
        }

        var result = mapper.Map<UpsertMedicalRecordResult>(entity);

        return result;
    }
}