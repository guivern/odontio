using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Budgets.Queries.GetBudgetById;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetBudgetByIdQuery: IRequest<ErrorOr<GetBudgetFullResult>>, IPatientResource
{
    public long Id { get; set; }
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}