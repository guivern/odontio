using Odontio.Application.Common.Interfaces;
using Odontio.Application.Payments.Common;

namespace Odontio.Application.Payments.Commands.UpdatePayment;

public class UpdatePaymentHandler(IApplicationDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdatePaymentCommand, ErrorOr<UpsertPaymentResult>>
{
    public async Task<ErrorOr<UpsertPaymentResult>> Handle(UpdatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await context.Payments.AsNoTracking()
            .Include(x => x.Budget)
            .Where(x => x.BudgetId == request.BudgetId)
            .Where(x => x.Budget.PatientId == request.PatientId)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (payment == null)
        {
            return Error.NotFound(description: "Payment not found");
        }

        payment = mapper.Map(request, payment);
        
        context.Payments.Entry(payment).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The payment was modified by another user");
        }

        var result = mapper.Map<UpsertPaymentResult>(payment);

        return result;
    }
}