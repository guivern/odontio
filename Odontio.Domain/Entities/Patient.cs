using System.ComponentModel.DataAnnotations.Schema;
using Odontio.Domain.Common;
using Odontio.Domain.Enums;

namespace Odontio.Domain.Entities;

public class Patient: BaseAuditableEntity
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly? Birthdate { get; set; }
    public Gender Gender { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? WorkAddress { get; set; }
    public string? WorkPhone { get; set; }
    public string? Ruc { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string? LastDentalVisit {get; set;}
    public string? ToothLossCause {get; set;}
    public string? BrushingFrequency {get; set;}
    public string? Observations {get; set;}
    
    public long? ReferredId  {get; set;}
    [ForeignKey(nameof(ReferredId))]
    public Patient? Referred {get; set;}
    
    public long WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
    
    public ICollection<MedicalCondition> MedicalConditions { get; } = new List<MedicalCondition>();
    public ICollection<PatientDisease> Diseases { get; } = new List<PatientDisease>();
    public ICollection<Diagnosis> Diagnoses { get; } = new List<Diagnosis>();
    public ICollection<Budget> Budgets { get; } = new List<Budget>();
    public ICollection<ScheduledAppointment> ScheduledAppointments { get; } = new List<ScheduledAppointment>();
}