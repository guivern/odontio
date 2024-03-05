using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Budgets.Commands.UpdateBudget;

public class UpdateBudgetHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateBudgetCommand, ErrorOr<UpsertBudgetResult>>
{
    public async Task<ErrorOr<UpsertBudgetResult>> Handle(UpdateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var budget = await context.Budgets
            .Include(x => x.PatientTreatments)
            .Where(x => x.PatientId == request.PatientId)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (budget == null)
        {
            return Error.NotFound(description: "Budget not found");
        }

        budget = mapper.Map(request, budget);

        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<UpsertBudgetResult>(budget);

        return result;
    }
}