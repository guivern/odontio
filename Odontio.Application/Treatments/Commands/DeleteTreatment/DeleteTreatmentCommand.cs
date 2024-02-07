using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Commands.DeleteTreatment;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeleteTreatmentCommand : IRequest<ErrorOr<Unit>>, IWorkspaceResource
{
    [FromRoute] public long Id { get; set; }
    [FromRoute] public long WorkspaceId { get; set; }
}