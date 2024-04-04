using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;

namespace Odontio.Application.MedicalConditionQuestions.Commands.UpdateMedicalConditionQuestion;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdateMedicalConditionQuestionCommand : IRequest<ErrorOr<UpsertMedicalConditionQuestion>>, IWorkspaceResource
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long WorkspaceId { get; set; }
}