namespace Odontio.API.Contracts.Appointments;

public class UpdateAppointmentRequest
{
    public DateTimeOffset? Date {get; set;}
    
    public ICollection<UpdateMedicalNoteRequest> MedicalNotes { get; set; } = new List<UpdateMedicalNoteRequest>();
}

public class UpdateMedicalNoteRequest : CreateMedicalNoteRequest
{
    public long Id { get; set; }
}