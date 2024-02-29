namespace Odontio.API.Contracts.ScheduledVisits;

public class CreateScheduledVisitRequest
{
    public DateTimeOffset Date { get; set; }
    public string? Description { get; set; }
}