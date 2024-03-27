namespace Odontio.Application.Appointments.Common;

public class GetAppointmentFullResult
{
    public long Id { get; set; }
    public DateTimeOffset Date {get; set;}
    public long PatientId { get; set; }
    public string PatientName { get; set; }
    
    public ICollection<GetMedicalNoteResult> MedicalNotes { get; set; } = new List<GetMedicalNoteResult>();
}

public class GetMedicalNoteResult
{
    public long Id { get; set; }
    public long PatientTreatmentId { get; set; }
    public long BudgetId { get; set; }
    public string TreatmentName { get; set; }
    public long AppointmentId { get; set; }
    public string? Description {get; set;}
    public string? Observations {get; set;}
}