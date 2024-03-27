namespace Odontio.Application.Appointments.Commands.DeleteAppointment;

public class DeleteAppointmentValidator : AbstractValidator<DeleteAppointmentCommand>
{
    public DeleteAppointmentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}