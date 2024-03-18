using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Theeth.Query.GetTeeth;

public class GetTheethQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetThoothResult>>>
{
    public string? Odontogram { get; set; }
}