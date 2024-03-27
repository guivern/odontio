namespace Odontio.Application.PatientTreatments.Common;

public class GetPatientTreatmentFullResult
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public string TreatmentName { get; set; }
    public long? ToothId { get; set; }
    public string? ToothName { get; set; }
    public decimal Cost { get; set; }
    public long BudgetId { get; set; }
    public string Status { get; set; }
    public long PatientId { get; set; }
    public string PatientName { get; set; }

    public List<GetMedicalNoteResultDto> MedicalNotes { get; set; } = new();
}

public class GetMedicalNoteResultDto
{
    public long Id { get; set; }
    public long AppointmentId { get; set; }
    public DateTimeOffset Date {get; set;}
    public string? Description { get; set; }
    public string? Observations { get; set; }
}