using Odontio.Domain.Common;
using Odontio.Domain.Enums;

namespace Odontio.Domain.Entities;

public class Payment: BaseAuditableEntity
{
    public long Id {get; set;}
    public DateTimeOffset Date {get; set;}
    public PaymentMethod PaymentMethod {get; set;}
    public decimal Amount {get; set;}
    public ReceiptType ReceiptType {get; set;}
    public string? ReceiptNumber {get; set;}
    
    public long BudgetId {get; set;}
    public Budget Budget {get; set;} = null!;
}