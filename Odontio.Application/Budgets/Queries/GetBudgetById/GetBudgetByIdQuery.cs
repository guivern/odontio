using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Budgets.Queries.GetBudgetById;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(Roles.Administrator))]
public class GetBudgetByIdQuery: IRequest<ErrorOr<GetBudgetResult>>, IPatientResource
{
    public long Id { get; set; }
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}