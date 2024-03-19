using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalRecords.Common;

namespace Odontio.Application.MedicalRecords.Queries.GetMedicalRecordById;

public class GetMedicalRecordByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMedicalRecordByIdQuery, ErrorOr<GetMedicalRecordFullResult>>
{
    public async Task<ErrorOr<GetMedicalRecordFullResult>> Handle(GetMedicalRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var medicalRecord = await context.MedicalRecords
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

        if (medicalRecord == null)
        {
            return Error.NotFound(description: "Medical record not found");
        }

        var result = mapper.Map<GetMedicalRecordFullResult>(medicalRecord);

        return result;
    }
}