using MapsterMapper;
using Odontio.API.Contracts.Patients;
using Odontio.Application.MedicalRecords.Queries.GetMedicalRecords;
using Odontio.Application.Patients.Commands.CreatePatient;
using Odontio.Application.Patients.Commands.DeletePatient;
using Odontio.Application.Patients.Commands.UpdatePatient;
using Odontio.Application.Patients.Queries.GetPatientById;
using Odontio.Application.Patients.Queries.GetPatients;
using Odontio.Application.PatientTreatments.Queries.GetPatientTreatments;
using Odontio.Application.Payments.Queries.GetPayments;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/[controller]")]
public class PatientsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var request = new GetPatientsQuery
        {
            WorkspaceId = workspaceId,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy
        };

        var result = await mediator.Send(request, cancellationToken);

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

    [HttpGet("{id}/PatientTreatments")]
    public async Task<IActionResult> GetPatientTreatments(long id, long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetPatientTreatmentsQuery
        {
            WorkspaceId = workspaceId,
            PatientId = id,
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

    [HttpGet("{id}/MedicalRecords")]
    public async Task<IActionResult> GetMedicalRecords(long id, long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetMedicalRecordsQuery
        {
            WorkspaceId = workspaceId,
            PatientId = id,
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

    [HttpGet("{id}/Payments")]
    public async Task<IActionResult> GetPayments(long id, long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentsQuery
        {
            WorkspaceId = workspaceId,
            PatientId = id,
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
}