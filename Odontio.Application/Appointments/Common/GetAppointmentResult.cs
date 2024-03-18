namespace Odontio.Application.Appointments.Common;

public class GetAppointmentResult
{
    public long Id { get; set; }
    public DateTimeOffset Date {get; set;}
    public long PatientId { get; set; }
    public string PatientName { get; set; }
}