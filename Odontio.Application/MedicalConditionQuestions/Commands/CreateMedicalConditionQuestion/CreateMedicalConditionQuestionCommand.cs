using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;

namespace Odontio.Application.MedicalConditionQuestions.Commands.CreateMedicalConditionQuestion;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class CreateMedicalConditionQuestionCommand : IRequest<ErrorOr<UpsertMedicalConditionQuestion>>, IWorkspaceResource
{
    public string Name { get; set; } = null!;
    public long WorkspaceId { get; set; }
}