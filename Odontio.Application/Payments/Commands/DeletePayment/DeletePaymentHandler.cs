using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Payments.Commands.DeletePayment;

public class DeletePaymentHandler(IApplicationDbContext context) : IRequestHandler<DeletePaymentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await context.Payments.FindAsync(request.Id);

        if (payment == null)
        {
            return Error.NotFound(description: "Payment not found");
        }

        context.Payments.Remove(payment);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}