namespace Odontio.Application.Patients.Queries.GetPatients;

public class GetPatientsResult
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string? Ruc { get; set; }
}