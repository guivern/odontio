using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Commands.DeletePatient;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeletePatientCommand : IRequest<ErrorOr<Unit>>, IWorkspaceResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}