namespace Odontio.Application.Diseases.Commands.DeleteDisease;

public class DeleteDiseaseValidator : AbstractValidator<DeleteDiseaseCommand>
{
    public DeleteDiseaseValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}