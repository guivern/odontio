namespace Odontio.Application.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdValidator : AbstractValidator<GetAppointmentByIdQuery>
{
    public GetAppointmentByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}