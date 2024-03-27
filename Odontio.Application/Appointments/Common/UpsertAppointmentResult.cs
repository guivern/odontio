namespace Odontio.Application.Appointments.Common;

public class UpsertAppointmentResult
{
    public long Id { get; set; }
    public DateTimeOffset Date {get; set;}
    public long PatientId { get; set; }
    
    public ICollection<MedicalNoteResult> MedicalNotes { get; set; } = new List<MedicalNoteResult>();
}

public class MedicalNoteResult
{
    public long Id { get; set; }
    public long PatientTreatmentId { get; set; }
    public long AppointmentId { get; set; }
    public string? Description {get; set;}
    public string? Observations {get; set;}
}