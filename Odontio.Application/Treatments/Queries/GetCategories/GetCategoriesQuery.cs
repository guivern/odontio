using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Domain.Enums;

namespace Odontio.Application.Treatments.Queries.GetCategories;


[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetCategoriesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetCategoriesResult>>>
{
}