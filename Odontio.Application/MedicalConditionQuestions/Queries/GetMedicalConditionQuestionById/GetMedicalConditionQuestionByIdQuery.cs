using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;

namespace Odontio.Application.MedicalConditionQuestions.Queries.GetMedicalConditionQuestionById;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetMedicalConditionQuestionByIdQuery : IRequest<ErrorOr<UpsertMedicalConditionQuestion>>, IWorkspaceResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
}