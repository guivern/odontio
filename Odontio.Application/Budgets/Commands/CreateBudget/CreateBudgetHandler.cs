using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.Budgets.Commands.CreateBudget;

public class CreateBudgetHandler(IApplicationDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider): IRequestHandler<CreateBudgetCommand, ErrorOr<UpsertBudgetResult>>
{
    public async Task<ErrorOr<UpsertBudgetResult>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = mapper.Map<Budget>(request);
        
        budget.Date = request.Date ?? dateTimeProvider.Today;
        budget.Status = BudgetStatus.Pending;
        budget.ExpirationDate = budget.Date.AddMonths(1);
        
        context.Budgets.Add(budget);
        await context.SaveChangesAsync(cancellationToken);
        
        var result = mapper.Map<UpsertBudgetResult>(budget);
        
        return result;
    }
}