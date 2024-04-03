namespace Odontio.Application.Diseases.Queries.GetDiseases;

public class GetDiseasesValidator : AbstractValidator<GetDiseasesQuery>
{
    public GetDiseasesValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}