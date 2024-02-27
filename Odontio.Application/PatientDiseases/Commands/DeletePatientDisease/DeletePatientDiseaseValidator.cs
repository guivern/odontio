namespace Odontio.Application.PatientDiseases.Commands.DeletePatientDisease;

public class DeletePatientDiseaseValidator : AbstractValidator<DeletePatientDiseaseCommand>
{
    public DeletePatientDiseaseValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}