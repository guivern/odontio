using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class Workspace: BaseAuditableEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Ruc { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? BusinessName { get; set; }

    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<Disease> Diseases { get; set; } = new List<Disease>();
    public ICollection<MedicalConditionQuestion> MedicalConditionQuestions { get; set; } = new List<MedicalConditionQuestion>();
}