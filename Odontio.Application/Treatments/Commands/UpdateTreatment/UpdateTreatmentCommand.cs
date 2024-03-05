using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Treatments.Commands.UpdateTreatment;

[ValidateWorkspace]
[RolesAuthorize(nameof(Roles.Administrator))]
public class UpdateTreatmentCommand : IRequest<ErrorOr<UpdateTreatmentResult>>, IWorkspaceResource
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public long CategoryId { get; set; }
    public long WorkspaceId { get; set; }
}