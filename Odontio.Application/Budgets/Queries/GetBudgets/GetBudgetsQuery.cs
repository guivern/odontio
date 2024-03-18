using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Queries.GetBudgets;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetBudgetsQuery: PagedListQueryBase, IRequest<ErrorOr<PagedList<GetBudgetResultDto>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long? PatientId { get; set; }
}