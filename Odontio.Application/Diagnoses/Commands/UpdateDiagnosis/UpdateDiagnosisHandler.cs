using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Diagnoses.Commands.UpdateDiagnosis;

public class UpdateDiagnosisHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateDiagnosisCommand, ErrorOr<UpsertDiagnosisResult>>
{
    public async Task<ErrorOr<UpsertDiagnosisResult>> Handle(UpdateDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        var diagnosis = await context.Diagnoses
            .Where(x => x.Id == request.Id && x.PatientId == request.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (diagnosis == null)
        {
            return Error.NotFound(description: "Diagnosis not found");
        }

        diagnosis = mapper.Map(request, diagnosis);
        
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The diagnosis was modified by another user");
        }

        var result = mapper.Map<UpsertDiagnosisResult>(diagnosis);

        return result;
    }
}