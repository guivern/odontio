using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

public class GetScheduledVisitByWorkspaceResult: UpsertScheduledVisitResult
{
    public string PatientName { get; set; }
}