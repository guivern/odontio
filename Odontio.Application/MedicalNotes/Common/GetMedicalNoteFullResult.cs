namespace Odontio.Application.MedicalNotes.Common;

public class GetMedicalNoteFullResult
{
    public long Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public long BudgetId { get; set; }
    public long PatientTreatmentId { get; set; }
    public string PatientName { get; set; }
    public string TreatmentName { get; set; }
    public long AppointmentId { get; set; }
    public string? Description {get; set;}
    public string? Observations {get; set;}
}