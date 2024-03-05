using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Queries.GetBudgetsByPatient;

public class GetBudgetsByPatientHandler(IApplicationDbContext context) : IRequestHandler<GetBudgetsByPatientQuery, PagedList<UpsertBudgetResult>>
{
    public async Task<PagedList<UpsertBudgetResult>> Handle(GetBudgetsByPatientQuery request, CancellationToken cancellationToken)
    {
        
        var query = context.Budgets
            .Include(x => x.Patient)
            .Include(x => x.PatientTreatments)
            .ThenInclude(x => x.Treatment)
            .Where(x => x.Patient.WorkspaceId == request.WorkspaceId)
            .Where(x => x.PatientId == request.PatientId)
            .ProjectToType<UpsertBudgetResult>()
            .AsNoTracking()
            .AsQueryable();

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