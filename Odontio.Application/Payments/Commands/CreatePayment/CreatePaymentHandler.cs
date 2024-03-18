using Odontio.Application.Common.Interfaces;
using Odontio.Application.Payments.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Payments.Commands.CreatePayment;

public class CreatePaymentHandler(IApplicationDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreatePaymentCommand, ErrorOr<UpsertPaymentResult>>
{
    public async Task<ErrorOr<UpsertPaymentResult>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = mapper.Map<Payment>(request);
        
        payment.Date = request.Date ?? dateTimeProvider.UtcNow;
        
        await context.Payments.AddAsync(payment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<UpsertPaymentResult>(payment);
        
        return result;
    }
}