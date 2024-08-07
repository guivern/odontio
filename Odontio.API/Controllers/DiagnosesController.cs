﻿using Odontio.API.Contracts.Diagnoses;
using Odontio.Application.Diagnoses.Commands.CreateDiagnosis;
using Odontio.Application.Diagnoses.Commands.DeleteDiagnosis;
using Odontio.Application.Diagnoses.Commands.UpdateDiagnosis;
using Odontio.Application.Diagnoses.Queries.GetDiagnoses;
using Odontio.Application.Diagnoses.Queries.GetDiagnosisById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/[controller]")]
public class DiagnosesController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(long workspaceId, long patientId, PaginationQueryParams pagination, CancellationToken cancellationToken)
    {
        var query = new GetDiagnosesQuery
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy
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
    
    [HttpGet("tooth/{toothId}")]
    public async Task<IActionResult> GetByPatientTooth(long workspaceId, long patientId, long toothId,
        CancellationToken cancellationToken)
    {
        var query = new GetDiagnosesQuery
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            ToothId = toothId,
            Page = 1,
            PageSize = -1
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long workspaceId, long patientId, long id,
        CancellationToken cancellationToken)
    {
        var query = new GetDiagnosisByIdQuery
        {
            Id = id,
            PatientId = patientId,
            WorkspaceId = workspaceId
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, long patientId, CreateDiagnosisRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateDiagnosisCommand>(request);
        
        command.WorkspaceId = workspaceId;
        command.PatientId = patientId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new { id = result.Id, workspaceId = command.WorkspaceId, patientId = command.PatientId }, result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long workspaceId, long patientId, long id,
        [FromBody] UpdateDiagnosisRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateDiagnosisCommand>(request);

        command.Id = id;
        command.PatientId = patientId;
        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long workspaceId, long patientId, long id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteDiagnosisCommand
        {
            Id = id,
            PatientId = patientId,
            WorkspaceId = workspaceId
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(),
            errors => Problem(errors)
        );
    }
}