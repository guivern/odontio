using Odontio.Application.Common.Attributes;

namespace Odontio.Application.Treatments.Common.CreateTreatment;

[RolesAuthorize(nameof(RolesEnum.User), nameof(RolesEnum.Administrator))]
public class CreateTreatmentCommand : IRequest<ErrorOr<CreateTreatmentResult>>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public long CategoryId { get; set; }

    public long WorkspaceId { get; set; }
}