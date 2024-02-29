namespace Odontio.API.Contracts.ScheduledVisits;

public class UpdateScheduledVisitRequest
{
    public DateTimeOffset Date { get; set; }
    public string? Description { get; set; }
}