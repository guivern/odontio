using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Budgets.Queries.GetBudgets;

public class GetBudgetsHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetBudgetsQuery, ErrorOr<PagedList<GetBudgetResult>>>
{
    public async Task<ErrorOr<PagedList<GetBudgetResult>>> Handle(GetBudgetsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Budgets
            .Include(x => x.Patient)
            .Include(x => x.Payments)
            .Include(x => x.PatientTreatments)
            .ThenInclude(x => x.Treatment)
            .AsNoTracking()
            .Where(x => x.Patient.WorkspaceId == request.WorkspaceId)
            .AsQueryable();

        if (request.PatientId != null)
        {
            var patientExits = await context.Patients.AnyAsync(x => x.Id == request.PatientId
                && x.WorkspaceId == request.WorkspaceId, cancellationToken);

            if (!patientExits)
                return Error.NotFound(description: "Patient not found in this workspace");
            
            query = query.Where(x => x.PatientId == request.PatientId);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            if (request.PatientId == null)
            {
                query = query.Filter(request.Filter, new List<string>
                {
                    $"{nameof(Budget.Patient)}.{nameof(Patient.FirstName)}",
                    $"{nameof(Budget.Patient)}.{nameof(Patient.LastName)}",
                    $"{nameof(Budget.Patient)}.{nameof(Patient.DocumentNumber)}",
                    $"{nameof(Budget.Patient)}.{nameof(Patient.Ruc)}",
                });
            }
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }
        
        var result = await PagedList<Budget>.CreateAsync(query, request.Page, request.PageSize);
        var dto = mapper.Map<PagedList<GetBudgetResult>>(result);
        
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
    }
}