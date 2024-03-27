using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalNotes.Common;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateMedicalNoteCommand, ErrorOr<UpsertMedicalNoteResult>>
{
    public async Task<ErrorOr<UpsertMedicalNoteResult>> Handle(CreateMedicalNoteCommand request,
        CancellationToken cancellationToken)
    {
        var entity = mapper.Map<MedicalNote>(request);

        context.MedicalNotes.Add(entity);

        var patientTreatment = await context.PatientTreatments
            .Include(x => x.Budget)
            .FirstAsync(x => x.Id == entity.PatientTreatmentId, cancellationToken);

        patientTreatment.Status = TreatmentStatus.InProgress;
        patientTreatment.Budget.Status = BudgetStatus.Approved;

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Error.Unexpected("An error occurred while creating the medical record. Please try again.");
        }

        var result = mapper.Map<UpsertMedicalNoteResult>(entity);

        return result;
    }
}