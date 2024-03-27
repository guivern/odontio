namespace Odontio.API.Contracts.Appointments;

public class CreateAppointmentRequest
{
    public DateTimeOffset? Date {get; set;}
    
    public ICollection<CreateMedicalNoteRequest> MedicalNotes { get; set; } = new List<CreateMedicalNoteRequest>();
}

public class CreateMedicalNoteRequest
{
    public long PatientTreatmentId { get; set; }
    public string Description {get; set;} = null!;
    public string? Observations {get; set;}
}