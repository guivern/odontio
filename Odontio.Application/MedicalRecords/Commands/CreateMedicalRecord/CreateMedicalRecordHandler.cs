using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalRecords.Common;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.MedicalRecords.Commands.CreateMedicalRecord;

public class CreateMedicalRecordHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateMedicalRecordCommand, ErrorOr<UpsertMedicalRecordResult>>
{
    public async Task<ErrorOr<UpsertMedicalRecordResult>> Handle(CreateMedicalRecordCommand request,
        CancellationToken cancellationToken)
    {
        var entity = mapper.Map<MedicalRecord>(request);

        context.MedicalRecords.Add(entity);

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

        var result = mapper.Map<UpsertMedicalRecordResult>(entity);

        return result;
    }
}