namespace Odontio.Application.MedicalConditionQuestions.Commands.DeleteMedicalConditionQuestion;

public class DeleteMedicalConditionQuestionValidator : AbstractValidator<DeleteMedicalConditionQuestionCommand>
{
    public DeleteMedicalConditionQuestionValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}