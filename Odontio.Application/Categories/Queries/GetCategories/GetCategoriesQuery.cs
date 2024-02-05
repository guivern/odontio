using MediatR;
using Odontio.Application.Categories.Common;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using ErrorOr;
using RolesEnum = Odontio.Domain.Enums.Roles;

namespace Odontio.Application.Categories.Queries.GetCategories;

public record GetCategoriesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetCategoryResult>>>, IAuthorizeable
{
    public string[] Roles => new[] { nameof(RolesEnum.User), nameof(RolesEnum.Administrator) };
}