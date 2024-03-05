using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.DeleteBudget;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeleteBudgetCommand : IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}