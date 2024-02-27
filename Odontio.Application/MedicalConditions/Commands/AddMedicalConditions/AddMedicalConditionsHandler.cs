using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.MedicalConditions.Common.AddMedicalConditions;

public class AddMedicalConditionsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<AddMedicalConditionsCommand, ErrorOr<List<MedicalConditionResult>>>
{
    public async Task<ErrorOr<List<MedicalConditionResult>>> Handle(AddMedicalConditionsCommand command,
        CancellationToken cancellationToken)
    {
        var medicalConditions = mapper.Map<List<MedicalCondition>>(command.MedicalConditions);

        foreach (var medicalCondition in medicalConditions)
        {
            medicalCondition.PatientId = command.PatientId;
        }

        await context.MedicalConditions.AddRangeAsync(medicalConditions, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<List<MedicalConditionResult>>(medicalConditions);

        return result;
    }
}