using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Domain.Enums;

namespace Odontio.Application.Categories.Queries.GetCategories;


[RolesAuthorize(nameof(Roles.Administrator))]
public class GetCategoriesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetCategoriesResult>>>
{
}