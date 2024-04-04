using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditionQuestions.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.MedicalConditionQuestions.Queries.GetMedicalConditionQuestions;

public class GetMedicalConditionQuestionsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMedicalConditionQuestionsQuery, ErrorOr<PagedList<UpsertMedicalConditionQuestion>>>
{
    public async Task<ErrorOr<PagedList<UpsertMedicalConditionQuestion>>> Handle(
        GetMedicalConditionQuestionsQuery request, CancellationToken cancellationToken)
    {
        var query = context.MedicalConditionQuestions
            .AsNoTracking()
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .ProjectToType<UpsertMedicalConditionQuestion>()
            .AsQueryable();


        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                nameof(MedicalConditionQuestion.Name),
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        return await PagedList<UpsertMedicalConditionQuestion>.CreateAsync(query, request.Page, request.PageSize);
    }
}