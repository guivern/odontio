using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalConditionQuestions.Commands.DeleteMedicalConditionQuestion;

public class DeleteMedicalConditionQuestionHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteMedicalConditionQuestionCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteMedicalConditionQuestionCommand request,
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

        context.MedicalConditionQuestions.Remove(entity);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            return Error.Conflict(description: "Can not delete medical condition question due to existing dependencies.");
        }

        return Unit.Value;
    }
}