using Odontio.Application.PatientTreatments.Common;

namespace Odontio.Application.Budgets.Common;

public class GetBudgetFullResult
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; } = null!;
    public DateOnly ExpirationDate { get; set; }
    public long PatientId { get; set; }
    public string PatientName { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal Balance { get; set; }

    public List<GetPatientTreatmentResultDto> PatientTreatments { get; set; } = new();
    public List<GetPaymentDto> Payments { get; set; } = new();
}

public class GetPatientTreatmentResultDto
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public string TreatmentName { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
    public string Status { get; set; }
    public List<GetMedicalNoteResultDto> MedicalNotes { get; set; } = new();
}

public class GetPaymentDto
{
    public long Id {get; set;}
    public DateTimeOffset? Date {get; set;}
    public string PaymentMethod {get; set;}
    public decimal Amount {get; set;}
    public string ReceiptType {get; set;}
    public string? ReceiptNumber {get; set;}
}