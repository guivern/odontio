using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Patients.Commands.DeletePatient;

[ValidateWorkspace]
[RolesAuthorize(nameof(Roles.Administrator))]
public class DeletePatientCommand : IRequest<ErrorOr<Unit>>, IWorkspaceResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
}