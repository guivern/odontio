using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.ScheduledVisits.Commands.CreateScheduledVisit;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class CreateScheduleVisitCommand : IRequest<ErrorOr<UpsertScheduledVisitResult>>, IPatientResource
{
    public DateTimeOffset Date { get; set; }
    public string? Description { get; set; }
    public long PatientId { get; set; }

    public long WorkspaceId { get; set; }
}