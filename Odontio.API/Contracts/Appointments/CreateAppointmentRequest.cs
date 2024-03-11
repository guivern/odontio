namespace Odontio.API.Contracts.Appointments;

public class CreateAppointmentRequest
{
    public DateTimeOffset? Date {get; set;}
    
    public ICollection<CreateMedicalRecordRequest> MedicalRecords { get; set; } = new List<CreateMedicalRecordRequest>();
}

public class CreateMedicalRecordRequest
{
    public long PatientTreatmentId { get; set; }
    public string? Description {get; set;}
    public string? Observations {get; set;}
}