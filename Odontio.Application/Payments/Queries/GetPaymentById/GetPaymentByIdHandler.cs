using Odontio.Application.Common.Interfaces;
using Odontio.Application.Payments.Common;

namespace Odontio.Application.Payments.Queries.GetPaymentById;

public class GetPaymentByIdHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetPaymentByIdQuery, ErrorOr<GetPaymentResult>>
{
    public async Task<ErrorOr<GetPaymentResult>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await context.Payments
            .Include(x => x.Budget)
            .ThenInclude(x => x.Patient)
            .Where(x => x.Budget.PatientId == request.PatientId)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (payment == null)
        {
            return Error.NotFound(description: "Payment not found");
        }

        var result = mapper.Map<GetPaymentResult>(payment);

        return result;
    }
}