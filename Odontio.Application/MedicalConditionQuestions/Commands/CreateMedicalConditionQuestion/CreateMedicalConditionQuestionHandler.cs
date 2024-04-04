using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.MedicalConditionQuestions.Commands.CreateMedicalConditionQuestion;

public class CreateMedicalConditionQuestionHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateMedicalConditionQuestionCommand, ErrorOr<UpsertMedicalConditionQuestion>>
{
    public async Task<ErrorOr<UpsertMedicalConditionQuestion>> Handle(CreateMedicalConditionQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<MedicalConditionQuestion>(request);

        context.MedicalConditionQuestions.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<UpsertMedicalConditionQuestion>(entity);
        
        return result;
    }
}