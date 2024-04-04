using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalConditionQuestions.Commands.DeleteMedicalConditionQuestion;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class DeleteMedicalConditionQuestionCommand : IRequest<ErrorOr<Unit>>, IWorkspaceResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
}