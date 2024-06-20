using System.ComponentModel.DataAnnotations.Schema;
using Odontio.Domain.Common;
using Odontio.Domain.Enums;

namespace Odontio.Domain.Entities;

public class Patient : BaseAuditableEntity
{
    public long Id { get; set; }
    
    public long WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
    
    #region Datos básicos

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly? Birthdate { get; set; }
    public Gender Gender { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string DocumentNumber { get; set; } = null!;

    public long? ReferredId { get; set; }
    [ForeignKey(nameof(ReferredId))] public Patient? Referred { get; set; }

    #endregion

    #region Datos laborales

    public string? Occupation { get; set; }
    public string? WorkCompany { get; set; }
    public string? WorkAddress { get; set; }
    public string? WorkPhone { get; set; }

    #endregion

    #region Datos de facturación

    public string? Ruc { get; set; }
    public string? BusinessName { get; set; }
    
    #endregion

    # region Antecedentes Odontológicos

    public string? LastDentalVisit { get; set; }
    public string? ToothLossCause { get; set; }
    public string? BrushingFrequency { get; set; }
    public string? Observations { get; set; }

    #endregion

    #region Historia Médica

    public ICollection<PatientDisease> Diseases { get; set; } = new List<PatientDisease>();
    public ICollection<MedicalCondition> MedicalConditions { get; set; } = new List<MedicalCondition>();

    #endregion

    public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<ScheduledVisit> ScheduledVisits { get; set; } = new List<ScheduledVisit>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}