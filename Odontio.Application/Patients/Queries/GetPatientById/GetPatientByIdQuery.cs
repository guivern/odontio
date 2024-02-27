using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Queries.GetPatientById;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.User), nameof(RolesEnum.Administrator))]
public class GetPatientByIdQuery : IRequest<ErrorOr<GetPatientByIdResult>>, IWorkspaceResource
{
    public long Id { get; init; }
    public long WorkspaceId { get; init; }
}