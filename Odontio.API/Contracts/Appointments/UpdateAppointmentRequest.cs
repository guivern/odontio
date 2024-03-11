namespace Odontio.API.Contracts.Appointments;

public class UpdateAppointmentRequest
{
    public DateTimeOffset? Date {get; set;}
    
    public ICollection<UpdateMedicalRecordRequest> MedicalRecords { get; set; } = new List<UpdateMedicalRecordRequest>();
}

public class UpdateMedicalRecordRequest : CreateMedicalRecordRequest
{
    public long Id { get; set; }
}