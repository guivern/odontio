using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Common;
using Odontio.Domain.Entities;

namespace Odontio.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAuthService _authService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeProvider dateTimeProvider,
        IAuthService authService) : base(options)
    {
        _authService = authService;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // -------------- Moved to DbContextInitializer ---------------
        // #region seeds
        // foreach (var role in (Roles[]) Enum.GetValues(typeof(Roles)))
        // {
        //     builder.Entity<Role>().HasData(new Role
        //     {
        //         Id = (long)role,
        //         Name = role.ToString(),
        //     });
        // }
        // #endregion
        
        // delete cascade for workspace and user relationship
        builder.Entity<Workspace>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Workspace)
            .OnDelete(DeleteBehavior.Cascade);

        // cannot delete a workspace if it has patients
        builder.Entity<Workspace>()
            .HasMany(x => x.Patients)
            .WithOne(x => x.Workspace)
            .OnDelete(DeleteBehavior.Restrict);

        // cannot delete a category if it has treatments
        builder.Entity<Category>()
            .HasMany(x => x.Treatments)
            .WithOne(x => x.Category)
            .OnDelete(DeleteBehavior.Restrict);

        // cannot delete a treatment if it has patient treatments (in a budget)
        builder.Entity<Treatment>()
            .HasMany(x => x.PatientTreatments)
            .WithOne(x => x.Treatment)
            .OnDelete(DeleteBehavior.Restrict);

        // cannot delete a patient if it has appointments
        builder.Entity<Patient>()
            .HasMany(x => x.Appointments)
            .WithOne(x => x.Patient)
            .OnDelete(DeleteBehavior.Restrict);

        // cannot delete a patient if it has budgets
        builder.Entity<Patient>()
            .HasMany(x => x.Budgets)
            .WithOne(x => x.Patient)
            .OnDelete(DeleteBehavior.Restrict);

        // cannot delete a budget if it has payments
        builder.Entity<Budget>()
            .HasMany(x => x.Payments)
            .WithOne(x => x.Budget)
            .OnDelete(DeleteBehavior.Restrict);

        // cannot delete a patient treatment if it has medical records
        builder.Entity<PatientTreatment>()
            .HasMany(x => x.MedicaNotes)
            .WithOne(x => x.PatientTreatment)
            .OnDelete(DeleteBehavior.Restrict);

        // cannot delete a role if it has users
        builder.Entity<Role>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Role)
            .OnDelete(DeleteBehavior.Restrict);
        
        // cannot delete a disease if it has patients
        builder.Entity<Disease>()
            .HasMany(x => x.PatientDiseases)
            .WithOne(x => x.Disease)
            .OnDelete(DeleteBehavior.Restrict);
        
        // defult value for IsActive
        builder.Entity<User>()
            .Property(x => x.IsActive)
            .HasDefaultValue(true);

        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = _dateTimeProvider.UtcNow;
        var currentUsername = _authService.GetCurrentUserName();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseAuditableEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.Created = now;
                        baseEntity.CreatedBy = currentUsername;
                        break;

                    case EntityState.Modified:
                        baseEntity.LastModified = now;
                        baseEntity.LastModifiedBy = currentUsername;
                        break;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Disease> Diseases { get; set; }
    public DbSet<PatientTreatment> PatientTreatments { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<MedicalNote> MedicalNotes { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalCondition> MedicalConditions { get; set; }
    public DbSet<MedicalConditionQuestion> MedicalConditionQuestions { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientDisease> PatientDiseases { get; set; }
    public DbSet<Tooth> Teeth { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ScheduledVisit> ScheduledVisits { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}