namespace Odontio.API.Contracts.Payments;

public class CreatePaymentRequest
{
    public DateTimeOffset? Date { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string ReceiptType { get; set; }
    public string? ReceiptNumber { get; set; }
}