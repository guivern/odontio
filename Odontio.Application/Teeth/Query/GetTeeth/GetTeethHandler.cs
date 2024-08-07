using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Teeth.Query.Common;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.Teeth.Query.GetTeeth;

public class GetTeethHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTeethQuery, ErrorOr<PagedList<GetToothResult>>>
{
    public async Task<ErrorOr<PagedList<GetToothResult>>> Handle(GetTeethQuery request, CancellationToken cancellationToken)
    {
        var query = context.Teeth.AsNoTracking().AsQueryable();
        
        if (request.Odontogram != null)
        {
            var odontogram = request.Odontogram == "Adulto" ? OdontogramType.Adulto : OdontogramType.Niño;
            query = query.Where(x => x.OdontogramType == odontogram);
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
        
        var result = await PagedList<Tooth>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);
        
        var dto = mapper.Map<PagedList<GetToothResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
    }
}