namespace Odontio.Application.Patients.Commands.DeletePatient;

public class DeletePatientValidator : AbstractValidator<DeletePatientCommand>
{
    public DeletePatientValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}