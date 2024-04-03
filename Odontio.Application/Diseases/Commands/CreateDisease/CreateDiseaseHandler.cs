using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Diseases.Commands.CreateDisease;

public class CreateDiseaseHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateDiseaseCommand, ErrorOr<UpsertDiseaseResult>>
{
    public async Task<ErrorOr<UpsertDiseaseResult>> Handle(CreateDiseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Disease>(request);

        context.Diseases.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<UpsertDiseaseResult>(entity);
    }
}