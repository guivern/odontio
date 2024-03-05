using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Queries.GetBudgetById;

public class GetBudgetByIdHandler(IApplicationDbContext context): IRequestHandler<GetBudgetByIdQuery, ErrorOr<UpsertBudgetResult>>
{
    public async Task<ErrorOr<UpsertBudgetResult>> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        var budget = await context.Budgets
            .Include(x => x.Patient)
            .Include(x => x.PatientTreatments)
            .ThenInclude(x => x.Treatment)
            .Where(x => x.Id == request.Id)
            .Where(x => x.PatientId == request.PatientId)
            .ProjectToType<UpsertBudgetResult>()
            .FirstOrDefaultAsync(cancellationToken);

        if (budget == null)
        {
            return Error.NotFound(description: "Budget not found.");
        }

        return budget;
    }
}