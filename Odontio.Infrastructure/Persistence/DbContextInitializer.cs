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
        await _context.SaveChangesAsync();
        
        await SeedAdminUsers();
        await SeedDiseases();
        await _context.SaveChangesAsync();
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
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Odontio.Infrastructure/Persistence/Data/Workspaces.json");
        var data = await File.ReadAllTextAsync(filePath);
        var workspaces = JsonSerializer.Deserialize<List<Workspace>>(data);
        var existingWorkspaces = await _context.Workspaces.AsNoTracking().ToListAsync();
        
        foreach (var workspace in workspaces)
        {
            var existingWorkspace = existingWorkspaces.FirstOrDefault(x => x.Id == workspace.Id && x.Name == workspace.Name);
            if (existingWorkspace == null)
            {
                _context.Workspaces.Add(workspace);
            }
        }
    }

    private async Task SeedAdminUsers()
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
                    Username = $"admin{workspace.Id}",
                    RoleId = (long) Roles.Administrator,
                    WorkspaceId = workspace.Id,
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                };

                _context.Users.Add(user);
            }    
        }
    }
    
    private async Task SeedDiseases()
    {
        if (await _context.Diseases.AsNoTracking().AnyAsync()) return;
        
        // path: Odontio.Infrastructure/Persistence/Data/Diseases.json
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Odontio.Infrastructure/Persistence/Data/Diseases.json");
        var data = await File.ReadAllTextAsync(filePath);
        var diseases = JsonSerializer.Deserialize<List<Disease>>(data);
        
        await _context.Diseases.AddRangeAsync(diseases);
    }
}