using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Queries.GetMedicalRecordPdf;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetMedicalRecordPdfQuery : IRequest<ErrorOr<byte[]>>, IWorkspaceResource
{
        public long Id { get; init; }
        public long WorkspaceId { get; init; }
}