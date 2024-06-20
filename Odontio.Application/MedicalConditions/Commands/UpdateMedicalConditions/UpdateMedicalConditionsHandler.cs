using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.MedicalConditions.Common.UpdateMedicalConditions;

public class UpdateMedicalConditionsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateMedicalConditionsCommand, ErrorOr<List<MedicalConditionResult>>>
{
    public async Task<ErrorOr<List<MedicalConditionResult>>> Handle(UpdateMedicalConditionsCommand command,
        CancellationToken cancellationToken)
    {
        var patient = await context.Patients.Include(x => x.MedicalConditions)
            .FirstOrDefaultAsync(x => x.Id == command.PatientId && x.WorkspaceId == command.WorkspaceId,
                cancellationToken);

        if (patient == null)
        {
            return Error.NotFound(description: "Patient not found.");
        }
        
        var medicalConditions = mapper.Map<List<MedicalCondition>>(command.MedicalConditions);

        foreach (var medicalCondition in medicalConditions)
        {
            medicalCondition.PatientId = command.PatientId;
        }

        patient.MedicalConditions = medicalConditions;
        
        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<List<MedicalConditionResult>>(medicalConditions);

        return result;
    }
}