using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;

namespace Odontio.Application.Categories.Queries.GetCategories;


[RolesAuthorize(nameof(RolesEnum.User), nameof(RolesEnum.Administrator))]
public class GetCategoriesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetCategoriesResult>>>
{
}