using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Payments.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Payments.Queries.GetPayments;

public class GetPaymentsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPaymentsQuery, ErrorOr<PagedList<UpsertPaymentResult>>>
{
    public async Task<ErrorOr<PagedList<UpsertPaymentResult>>> Handle(GetPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Payments
            .Include(x => x.Budget)
            .ThenInclude(x => x.Patient)
            .Where(x => x.Budget.Patient.WorkspaceId == request.WorkspaceId);

        if (request.PatientId.HasValue)
        {
            var patientExists =
                await context.Patients.AnyAsync(x => x.Id == request.PatientId && x.WorkspaceId == request.WorkspaceId,
                    cancellationToken);

            if (!patientExists)
                return Error.NotFound(description: "Patient not found");

            query = query.Where(x => x.Budget.PatientId == request.PatientId);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(Payment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.FirstName)}",
                $"{nameof(Payment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.LastName)}",
                $"{nameof(Payment.Budget)}.{nameof(Budget.Patient)}.{nameof(Patient.DocumentNumber)}"
            });
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<Payment>.CreateAsync(query, request.Page, request.PageSize);

        var dto = mapper.Map<PagedList<UpsertPaymentResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;

        return dto;
    }
}