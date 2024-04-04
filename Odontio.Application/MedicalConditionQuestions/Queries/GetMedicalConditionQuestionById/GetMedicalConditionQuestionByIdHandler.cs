using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;

namespace Odontio.Application.MedicalConditionQuestions.Queries.GetMedicalConditionQuestionById;

public class GetMedicalConditionQuestionByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMedicalConditionQuestionByIdQuery, ErrorOr<UpsertMedicalConditionQuestion>>
{
    public async Task<ErrorOr<UpsertMedicalConditionQuestion>> Handle(GetMedicalConditionQuestionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await context.MedicalConditionQuestions
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.WorkspaceId == request.WorkspaceId)
            .ProjectToType<UpsertMedicalConditionQuestion>()
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "Medical condition question not found");
        }

        return entity;
    }
}