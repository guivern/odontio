using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalNotes.Common;

namespace Odontio.Application.MedicalNotes.Queries.GetMedicalNotes;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetMedicalNotesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetMedicalNoteFullResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long? PatientId { get; set; }
}