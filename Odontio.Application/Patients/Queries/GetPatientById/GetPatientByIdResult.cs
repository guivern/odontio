using Odontio.Application.MedicalConditions.Common;

namespace Odontio.Application.Patients.Queries.GetPatientById;

public class GetPatientByIdResult
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly? Birthdate { get; set; }
    public string Gender { get; set; } = null!;
    public string? MaritalStatus { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Occupation { get; set; }
    public string? WorkCompany { get; set; }
    public string? WorkAddress { get; set; }
    public string? WorkPhone { get; set; }
    public string? Ruc { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string? LastDentalVisit {get; set;}
    public string? ToothLossCause {get; set;}
    public string? BrushingFrequency {get; set;}
    public string? Observations {get; set;}
    public long? ReferredId  {get; set;}
}