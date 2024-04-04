using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;

namespace Odontio.Application.MedicalConditionQuestions.Commands.UpdateMedicalConditionQuestion;

public class UpdateMedicalConditionQuestionHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateMedicalConditionQuestionCommand, ErrorOr<UpsertMedicalConditionQuestion>>
{
    public async Task<ErrorOr<UpsertMedicalConditionQuestion>> Handle(UpdateMedicalConditionQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.MedicalConditionQuestions
            .Where(x => x.Id == request.Id)
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "Medical condition question not found");
        }

        entity = mapper.Map(request, entity);

        context.MedicalConditionQuestions.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The medical condition question was modified by another user");
        }

        var result = mapper.Map<UpsertMedicalConditionQuestion>(entity);

        return result;
    }
}