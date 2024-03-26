using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Payments.Commands.DeletePayment;

public class DeletePaymentHandler(IApplicationDbContext context) : IRequestHandler<DeletePaymentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await context.Payments
            .Include(x => x.Budget)
            .Where(x => x.Id == request.Id)
            .Where(x => x.BudgetId == request.BudgetId)
            .Where(x => x.Budget.PatientId == request.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (payment == null)
        {
            return Error.NotFound(description: "Payment not found");
        }

        context.Payments.Remove(payment);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}