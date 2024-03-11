using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Queries.GetBudgetsByWorkspace;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetBudgetsByWorkspaceQuery: PagedListQueryBase, IRequest<PagedList<GetBudgetResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
}