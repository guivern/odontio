﻿using Odontio.Application.PatientTreatments.Common;

namespace Odontio.Application.Budgets.Common;

public class GetBudgetFullResult
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; } = null!;
    public DateOnly ExpirationDate { get; set; }
    public long PatientId { get; set; }
    public string? Observations { get; set; }
    public string PatientName { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal Balance { get; set; }

    public List<GetBudgetDetailDto> Details { get; set; } = new();
    public List<GetPaymentDto> Payments { get; set; } = new();
}

public class GetBudgetDetailDto
{
    public long Id { get; set; }
    public BudgetTreatmentDto Treatment { get; set; } = null!;
    public BudgetDiagnosisDto? Diagnosis { get; set; }
    public decimal Cost { get; set; }
    public string Status { get; set; }
    public string? Observations { get; set; }
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