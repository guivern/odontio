using Odontio.API.Contracts.Users;
using Odontio.Application.Profile.Commands.UpdateProfile;
using Odontio.Application.Profile.Queries.GetProfile;
using Odontio.Application.Users.Commands.CreateUser;
using Odontio.Application.Users.Commands.DeleteUser;
using Odontio.Application.Users.Commands.UpdateUser;
using Odontio.Application.Users.Queries.GetUserById;
using Odontio.Application.Users.Queries.GetUsers;

namespace Odontio.API.Controllers;

[Route("api/v1/[controller]")]
public class UsersController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers(PaginationQueryParams pagination, [FromQuery] bool? onlyDoctors, 
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery
        {
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy,
            OnlyDoctors = onlyDoctors
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result =>
            {
                Response.AddPaginationHeader(result.PageNumber, result.PageSize, result.TotalCount, result.TotalPages);
                return Ok(result);
            },
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateUserCommand>(request);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById), new { id = result.Id }, result),
            errors => Problem(errors)
        );
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateUserCommand>(request);
        command.Id = id;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { Id = id };
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(
            result => NoContent(),
            errors => Problem(errors)
        );
    }
    
    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetProfile(long id, CancellationToken cancellationToken)
    {
        var query = new GetProfileQuery { Id = id };
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpPatch("{id}/profile")]
    public async Task<IActionResult> UpdateProfile(long id, UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateProfileCommand>(request);
        command.Id = id;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
}