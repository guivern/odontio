using Odontio.Application.Patients.Commands.CreatePatient;
using Odontio.Application.Patients.Queries.GetPatientById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/[controller]")]
public class PatientsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(GetPatientByIdQuery request)
    {
        var result = await mediator.Send(request);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, CreatePatientCommand command)
    {
        command.WorkspaceId = workspaceId;
        var result = await mediator.Send(command);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new GetPatientByIdQuery { Id = result.Id, WorkspaceId = workspaceId }, result),
            errors => Problem(errors)
        );
    }
}