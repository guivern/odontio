namespace Odontio.Application.ScheduledVisits.Common;

public class GetScheduledVisitResult
{
    public long Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Description { get; set; }
    public long PatientId { get; set; }
    public string PatientName { get; set; } = null!;
}