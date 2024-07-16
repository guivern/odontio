using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Patients.Queries.GetPatients;

public class GetPatientsHandler(IApplicationDbContext context)
    : IRequestHandler<GetPatientsQuery, ErrorOr<PagedList<GetPatientsResult>>>
{
    public async Task<ErrorOr<PagedList<GetPatientsResult>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Patients
            .AsNoTracking()
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .ProjectToType<GetPatientsResult>()
            .AsQueryable();


        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                nameof(Patient.FirstName),
                nameof(Patient.LastName),
                nameof(Patient.LastName),
                nameof(Patient.DocumentNumber),
                nameof(Patient.Ruc),
                nameof(Patient.Email),
                nameof(Patient.Phone),
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        return await PagedList<GetPatientsResult>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);
    }
}