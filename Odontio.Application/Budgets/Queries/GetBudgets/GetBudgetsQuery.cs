using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Queries.GetBudgets;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetBudgetsQuery: PagedListQueryBase, IRequest<ErrorOr<PagedList<GetBudgetResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long? PatientId { get; set; }
}