using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.Theeth.Query.GetTeeth;

public class GetTheethHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTheethQuery, ErrorOr<PagedList<GetThoothResult>>>
{
    public async Task<ErrorOr<PagedList<GetThoothResult>>> Handle(GetTheethQuery request, CancellationToken cancellationToken)
    {
        var query = context.Teeth.AsQueryable();
        
        if (request.Odontogram != null)
        {
            var odontogram = request.Odontogram == "Adult" ? Odontogram.Adult : Odontogram.Child;
            query = query.Where(x => x.Odontogram == odontogram);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                nameof(Tooth.Name),
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }
        
        var result = await PagedList<Tooth>.CreateAsync(query, request.Page, request.PageSize);
        
        var dto = mapper.Map<PagedList<GetThoothResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
    }
}