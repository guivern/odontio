using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Budgets.Queries.GetBudgetsByPatient;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(Roles.Administrator))]
public class GetBudgetsByPatientQuery: PagedListQueryBase, IRequest<PagedList<UpsertBudgetResult>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}