using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.ScheduledVisits.Commands.UpdateScheduledVisit;

public class UpdateScheduledVisitValidator: AbstractValidator<UpdateScheduledVisitCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateScheduledVisitValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Date).NotEmpty()
            .MustAsync(BeAvailableDate).WithMessage("Not available");
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
    
    private async Task<bool> BeAvailableDate(UpdateScheduledVisitCommand arg1, DateTimeOffset arg2, ValidationContext<UpdateScheduledVisitCommand> arg3, CancellationToken arg4)
    {
        // validate if the date is available in the workspace
        var isAvailable = !await _context.ScheduledVisits
            .Include(x => x.Patient)
            .AnyAsync(x => x.Date == arg2 && x.Id != arg1.Id && x.Patient.WorkspaceId == arg1.WorkspaceId, arg4);
        
        return isAvailable;
    }
}