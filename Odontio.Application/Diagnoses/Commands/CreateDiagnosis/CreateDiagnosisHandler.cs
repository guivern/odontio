using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Diagnoses.Commands.CreateDiagnosis;

public class CreateDiagnosisHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateDiagnosisCommand, ErrorOr<UpsertDiagnosisResult>>
{
    public async Task<ErrorOr<UpsertDiagnosisResult>> Handle(CreateDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        request.Date ??= DateOnly.FromDateTime(DateTime.UtcNow);
        
        var diagnosis = mapper.Map<Diagnosis>(request);
        
        context.Diagnoses.Add(diagnosis);
        
        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<UpsertDiagnosisResult>(diagnosis);

        return result;
    }
}