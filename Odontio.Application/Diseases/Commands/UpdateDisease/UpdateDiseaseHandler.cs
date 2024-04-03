using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;

namespace Odontio.Application.Diseases.Commands.UpdateDisease;

public class UpdateDiseaseHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateDiseaseCommand, ErrorOr<UpsertDiseaseResult>>
{
    public async Task<ErrorOr<UpsertDiseaseResult>> Handle(UpdateDiseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Diseases
            .Where(x => x.Id == request.Id)
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "Disease not found");
        }

        entity = mapper.Map(request, entity);
        
        context.Diseases.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The disease was modified by another user");
        }
        
        var result = mapper.Map<UpsertDiseaseResult>(entity);
        
        return result;
    }
}