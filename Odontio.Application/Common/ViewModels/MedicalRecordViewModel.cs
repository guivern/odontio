namespace Odontio.Application.Common.ViewModels;

public class MedicalRecordViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly? Birthdate { get; set; }
    public string Gender { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? WorkAddress { get; set; }
    public string? WorkPhone { get; set; }
    public string? Ruc { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string? LastDentalVisit { get; set; }
    public string? ToothLossCause { get; set; }
    public string? BrushingFrequency { get; set; }
    public string? Observations { get; set; }
    public string? ReferrerName { get; set; }
    public long WorkspaceId { get; set; }

    public List<MedicalConditionViewModel> MedicalConditions { get; set; } = new List<MedicalConditionViewModel>();
    public List<PatientDiseaseViewModel> Diseases { get; set; } = new List<PatientDiseaseViewModel>();
}

public class MedicalConditionViewModel
{
    public string ConditionType { get; set; } = null!;
    public bool? HasCondition { get; set; }
    public string? Description { get; set; }
}

public class PatientDiseaseViewModel
{
    public bool HasCondition { get; set; }
    public string Name { get; set; }
}