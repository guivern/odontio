using Odontio.Application.Common.Interfaces;
using Odontio.Application.Workspaces.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Workspaces.Commands.CreateWorkspace;

public class CreateWorkspaceHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateWorkspaceCommand, ErrorOr<UpsertWorkspaceResult>>
{
    public async Task<ErrorOr<UpsertWorkspaceResult>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Workspace>(request);
        
        context.Workspaces.Add(entity);
        
        await context.SaveChangesAsync(cancellationToken);
        
        var dto = mapper.Map<UpsertWorkspaceResult>(entity);
        
        return dto;
    }
}