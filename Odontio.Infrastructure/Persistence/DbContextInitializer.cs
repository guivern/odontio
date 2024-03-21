using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Infrastructure.Persistence;

public class DbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DbContextInitializer> _logger;
    private readonly IAuthService _authService;

    public DbContextInitializer(ApplicationDbContext context, ILogger<DbContextInitializer> logger, IAuthService authService)
    {
        _context = context;
        _logger = logger;
        _authService = authService;
    }
    
    // #pragma warning disable CA2255
    // [ModuleInitializer]
    // #pragma warning restore CA2255
    // public static void Initialize()
    // {
    //     AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    // }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
    
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Seed data here
        await SeedRoles();
        await SeedWorkspaces();
        await SeedCategories();
        await _context.SaveChangesAsync();

        await SeedAdmin();
        await SeedUsers();
        await SeedDiseases();
        await SeedTreatments();
        await SeedTeeth();
        await _context.SaveChangesAsync();
    }

    private async Task SeedTeeth()
    {
        if (await _context.Teeth.AsNoTracking().AnyAsync()) return;
        
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Odontio.Infrastructure/Persistence/Data/SeedTeeth.json");
        var data = await File.ReadAllTextAsync(filePath);
        var teethFromJson = JsonSerializer.Deserialize<List<Tooth>>(data);
        
        await _context.Teeth.AddRangeAsync(teethFromJson);
    }

    private async Task SeedTreatments()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Odontio.Infrastructure/Persistence/Data/SeedTreatments.json");
        var data = await File.ReadAllTextAsync(filePath);
        var treatmentsFromJson = JsonSerializer.Deserialize<List<Treatment>>(data);
        
        var workspacesWithoutTreatments = await _context.Workspaces
            .Include(x => x.Treatments)
            .Where(x => x.Treatments.Count == 0)
            .AsNoTracking()
            .ToListAsync();
        
        var treatmentsToAdd = new List<Treatment>();
        foreach (var workspace in workspacesWithoutTreatments)
        {
            foreach (var treatment in treatmentsFromJson)
            {
                treatmentsToAdd.Add(new Treatment
                {
                    Name = treatment.Name,
                    Description = treatment.Description,
                    Cost = treatment.Cost,
                    CategoryId = treatment.CategoryId,
                    WorkspaceId = workspace.Id,
                });
            }            
        }
        
        await _context.Treatments.AddRangeAsync(treatmentsToAdd);
    }

    private async Task SeedCategories()
    {
        if (await _context.Categories.AsNoTracking().AnyAsync()) return;
        
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Odontio.Infrastructure/Persistence/Data/SeedCategories.json");
        var data = await File.ReadAllTextAsync(filePath);
        var categoriesFromJson = JsonSerializer.Deserialize<List<Category>>(data);
        
        await _context.Categories.AddRangeAsync(categoriesFromJson);
    }

    private async Task SeedRoles()
    {
        var rolesEnum = (Roles[]) Enum.GetValues(typeof(Roles));
        var existingRoles = await _context.Roles.AsNoTracking().ToListAsync();
        foreach (var role in rolesEnum)
        {
            var existingRole = existingRoles.FirstOrDefault(x => x.Id == (long) role && x.Name == role.ToString());
            if (existingRole == null)
            {
                _context.Roles.Add(new Role
                {
                    Id = (long) role,
                    Name = role.ToString(),
                });
            }
        }
    }

    private async Task SeedWorkspaces()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Odontio.Infrastructure/Persistence/Data/SeedWorkspaces.json");
        var data = await File.ReadAllTextAsync(filePath);
        var workspacesJson = JsonSerializer.Deserialize<List<Workspace>>(data);
        var existingWorkspaces = await _context.Workspaces.AsNoTracking().ToListAsync();
        
        foreach (var workspace in workspacesJson)
        {
            var existingWorkspace = existingWorkspaces.FirstOrDefault(x => x.Id == workspace.Id && x.Name == workspace.Name);
            if (existingWorkspace == null)
            {
                _context.Workspaces.Add(workspace);
            }
        }
    }

    private async Task SeedUsers()
    {
        var workspaces = await _context.Workspaces.AsNoTracking().ToListAsync();
        foreach (var workspace in workspaces)
        {
            if (!await _context.Users.AsNoTracking().AnyAsync(x => x.WorkspaceId == workspace.Id))
            {
                var password = $@"Worksp@c3_{workspace.Id}";
                var passwordSalt = _authService.GeneratePasswordSalt();
                var passwordHash = _authService.GeneratePasswordHash(password, passwordSalt);

                var user = new User
                {
                    Username = $"user{workspace.Id}",
                    RoleId = (long) Roles.Doctor,
                    WorkspaceId = workspace.Id,
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                };

                _context.Users.Add(user);
            }    
        }
    }
    
    private async Task SeedAdmin()
    {
        if (!await _context.Users.AsNoTracking().AnyAsync(x => x.RoleId == (long) Roles.Administrator))
        {
            var password = $@"changeThisPassword";
            var passwordSalt = _authService.GeneratePasswordSalt();
            var passwordHash = _authService.GeneratePasswordHash(password, passwordSalt);

            var user = new User
            {
                Username = $"admin",
                RoleId = (long) Roles.Administrator,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
            };

            _context.Users.Add(user);
        }
    }
    
    private async Task SeedDiseases()
    {
        if (await _context.Diseases.AsNoTracking().AnyAsync()) return;
        
        // path: Odontio.Infrastructure/Persistence/Data/Diseases.json
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Odontio.Infrastructure/Persistence/Data/SeedDiseases.json");
        var data = await File.ReadAllTextAsync(filePath);
        var diseases = JsonSerializer.Deserialize<List<Disease>>(data);
        
        await _context.Diseases.AddRangeAsync(diseases);
    }
}