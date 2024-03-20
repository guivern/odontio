using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Payments.Commands.UpdatePayment;

public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePaymentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.PaymentMethod).NotNull().NotEmpty()
            .Must(BeValidPaymentMethod).WithMessage("Payment method is not valid");
        RuleFor(x => x.Amount).NotEmpty()
            .Must(x => x > 0).WithMessage("Amount must be greater than 0")
            .MustAsync(LessThanOrEqualToBalance).WithMessage("Exceeds the balance");
        RuleFor(x => x.ReceiptType).NotNull().NotEmpty()
            .Must(BeValidReceiptType).WithMessage("Receipt type is not valid");
        RuleFor(x => x.BudgetId).GreaterThan(0)
            .MustAsync(BudgetExits).WithMessage("Budget does not exist");
        RuleFor(x => x.Date).NotNull().NotEmpty();
    }
    
    private async Task<bool> LessThanOrEqualToBalance(UpdatePaymentCommand arg1, decimal arg2, CancellationToken arg3)
    {
        var totalCost = await _context.Budgets
            .AsNoTracking()
            .Include(x => x.PatientTreatments)
            .Where(x => x.Id == arg1.BudgetId)
            .SumAsync(x => x.PatientTreatments.Sum(x => x.Cost), cancellationToken: arg3);
        
        var totalPayments = await _context.Payments
            .AsNoTracking()
            .Where(x => x.BudgetId == arg1.BudgetId)
            .Where(x => x.Id != arg1.Id)
            .SumAsync(x => x.Amount, cancellationToken: arg3);
        
        var balance = totalCost - totalPayments;
        
        return arg2 <= balance;
    }

    private async Task<bool> BudgetExits(UpdatePaymentCommand arg1, long arg2, CancellationToken arg3)
    {
        var exists = await _context.Budgets
            .AsNoTracking()
            .AnyAsync(x => x.Id == arg2 && x.PatientId == arg1.PatientId, arg3);
        
        return exists;
    }

    private bool BeValidReceiptType(string arg)
    {
        return Enum.TryParse<ReceiptType>(arg, out _);
    }

    private bool BeValidPaymentMethod(string arg)
    {
        return Enum.TryParse<PaymentMethod>(arg, out _);
    }
}