﻿namespace Odontio.Application.Payments.Common;

public class UpsertPaymentResult
{
    public long Id {get; set;}
    public DateTimeOffset? Date {get; set;}
    public string PaymentMethod {get; set;}
    public decimal Amount {get; set;}
    public string ReceiptType {get; set;}
    public string? ReceiptNumber {get; set;}
    public long BudgetId {get; set;}
}