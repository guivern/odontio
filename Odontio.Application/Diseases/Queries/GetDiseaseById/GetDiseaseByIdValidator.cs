namespace Odontio.Application.Diseases.Queries.GetDiseaseById;

public class GetDiseaseByIdValidator : AbstractValidator<GetDiseaseByIdQuery>
{
    public GetDiseaseByIdValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}