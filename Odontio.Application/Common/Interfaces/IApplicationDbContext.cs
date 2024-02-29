using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Odontio.Domain.Entities;

namespace Odontio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    DbSet<Disease> Diseases { get; set; }
    DbSet<PatientTreatment> PatientTreatments { get; set; }
    DbSet<Budget> Budgets { get; set; }
    DbSet<Appointment> Appointments { get; set; }
    DbSet<MedicalCondition> MedicalConditions { get; set; }
    DbSet<Diagnosis> Diagnoses { get; set; }
    DbSet<Patient> Patients { get; set; }
    DbSet<PatientDisease> PatientDiseases { get; set; }
    DbSet<Tooth> Teeth { get; set; }
    DbSet<Workspace> Workspaces { get; set; }
    DbSet<Treatment> Treatments { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Payment> Payments { get; set; }
    DbSet<ScheduledVisit> ScheduledVisits { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
}