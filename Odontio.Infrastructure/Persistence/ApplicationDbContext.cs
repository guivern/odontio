using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Common;
using Odontio.Domain.Entities;

namespace Odontio.Infrastructure.Persistence;

public class ApplicationDbContext:  DbContext, IApplicationDbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
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
        
        builder.Entity<Workspace>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Workspace)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Workspace>()
            .HasMany(x => x.Patients)
            .WithOne(x => x.Workspace)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Patient>()
            .HasMany(x => x.MedicalRecords)
            .WithOne(x => x.Patient)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<MedicalRecord>()
            .HasMany(x => x.Payments)
            .WithOne(x => x.MedicalRecord)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<MedicalRecord>()
            .HasMany(x => x.Appointments)
            .WithOne(x => x.MedicalRecord)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Role>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Role)
            .OnDelete(DeleteBehavior.Restrict);
        
        base.OnModelCreating(builder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = _dateTimeProvider.UtcNow;
        var currentUsername = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        
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
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<MedicalCondition> MedicalConditions { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientDisease> PatientDiseases { get; set; }
    public DbSet<Tooth> Teeth { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}