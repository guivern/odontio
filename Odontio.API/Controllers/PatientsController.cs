using MapsterMapper;
using Odontio.API.Contracts.Patients;
using Odontio.Application.Patients.Commands.CreatePatient;
using Odontio.Application.Patients.Commands.DeletePatient;
using Odontio.Application.Patients.Commands.UpdatePatient;
using Odontio.Application.Patients.Queries.GetPatientById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/[controller]")]
public class PatientsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, long workspaceId, CancellationToken cancellationToken)
    {
        var request = new GetPatientByIdQuery { Id = id, WorkspaceId = workspaceId };
        var result = await mediator.Send(request, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, CreatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreatePatientCommand>(request);

        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new GetPatientByIdQuery { Id = result.Id, WorkspaceId = command.WorkspaceId }, result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long id, long workspaceId, UpdatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdatePatientCommand>(request);

        command.Id = id;
        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, long workspaceId, CancellationToken cancellationToken)
    {
        var command = new DeletePatientCommand { Id = id, WorkspaceId = workspaceId };
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(),
            errors => Problem(errors)
        );
    }
}