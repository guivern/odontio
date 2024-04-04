using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;

namespace Odontio.Application.MedicalConditionQuestions.Queries.GetMedicalConditionQuestions;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetMedicalConditionQuestionsQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<UpsertMedicalConditionQuestion>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
}