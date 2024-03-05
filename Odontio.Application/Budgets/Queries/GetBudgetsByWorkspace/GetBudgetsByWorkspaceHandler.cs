using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Budgets.Queries.GetBudgetsByWorkspace;

public class GetBudgetsByWorkspaceHandler(IApplicationDbContext context) : IRequestHandler<GetBudgetsByWorkspaceQuery, PagedList<UpsertBudgetResult>>
{
    public async Task<PagedList<UpsertBudgetResult>> Handle(GetBudgetsByWorkspaceQuery request, CancellationToken cancellationToken)
    {
        var query = context.Budgets
            .Include(x => x.Patient)
            .Include(x => x.PatientTreatments)
            .ThenInclude(x => x.Treatment)
            .AsNoTracking()
            .Where(x => x.Patient.WorkspaceId == request.WorkspaceId)
            .ProjectToType<UpsertBudgetResult>()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(Budget.Patient)}.{nameof(Patient.FirstName)}",
                $"{nameof(Budget.Patient)}.{nameof(Patient.LastName)}",
                $"{nameof(Budget.Patient)}.{nameof(Patient.DocumentNumber)}",
                $"{nameof(Budget.Patient)}.{nameof(Patient.Ruc)}",
            });
        }
        
        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Where(x => x.PatientTreatments
                .Any(t => t.TreatmentName.ToLower().Contains(request.Filter.ToLower())));
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }
        
        return await PagedList<UpsertBudgetResult>.CreateAsync(query, request.Page, request.PageSize);
    }
}