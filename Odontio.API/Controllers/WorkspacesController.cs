using Odontio.API.Contracts.Workspaces;
using Odontio.Application.Appointments.Queries.GetAppointments;
using Odontio.Application.Budgets.Queries.GetBudgets;
using Odontio.Application.Common.Helpers;
using Odontio.Application.MedicalRecords.Queries.GetMedicalRecords;
using Odontio.Application.PatientTreatments.Queries.GetPatientTreatments;
using Odontio.Application.Payments.Queries.GetPayments;
using Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;
using Odontio.Application.Workspaces.Commands.CreateWorkspace;
using Odontio.Application.Workspaces.Commands.DeleteWorkspace;
using Odontio.Application.Workspaces.Commands.UpdateWorkspace;
using Odontio.Application.Workspaces.Queries.GetWorkspaceById;
using Odontio.Application.Workspaces.Queries.GetWorkspaces;

namespace Odontio.API.Controllers;

[Route("api/v1/[controller]")]
public class WorkspacesController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var query = new GetWorkspaceByIdQuery { Id = id };
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkspaces(PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetWorkspacesQuery
        {
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

    [HttpPost]
    public async Task<IActionResult> CreateWorkspace(CreateWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateWorkspaceCommand>(request);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById), new { id = result.Id }, result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateWorkspace(long id, UpdateWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateWorkspaceCommand>(request);
        command.Id = id;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkspace(long id, CancellationToken cancellationToken)
    {
        var command = new DeleteWorkspaceCommand { Id = id };
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(
            result => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}/ScheduledVisits")]
    public async Task<IActionResult> GetScheduledVisits(long id, DateOnly? startDate, DateOnly? endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetScheduledVisitsByWorkspaceQuery
        {
            WorkspaceId = id,
            DateRange = new DateRange
            {
                StartDate = startDate,
                EndDate = endDate
            }
        };

        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}/Appointments")]
    public async Task<IActionResult> GetAppointments(long id, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetAppointmentsQuery
        {
            WorkspaceId = id,
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

    [HttpGet("{id}/Budgets")]
    public async Task<IActionResult> GetBudgets(long id, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetBudgetsQuery()
        {
            WorkspaceId = id,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy,
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

    [HttpGet("{id}/PatientTreatments")]
    public async Task<IActionResult> GetPatientTreatments(long id, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetPatientTreatmentsQuery()
        {
            WorkspaceId = id,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy,
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
    public async Task<IActionResult> GetMedicalRecords(long id, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetMedicalRecordsQuery()
        {
            WorkspaceId = id,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy,
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
    public async Task<IActionResult> GetPayments(long id, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentsQuery()
        {
            WorkspaceId = id,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy,
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