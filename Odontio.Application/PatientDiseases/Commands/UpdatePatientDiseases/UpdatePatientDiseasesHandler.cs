using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.PatientDiseases.Commands.UpdatePatientDiseases;

public class UpdatePatientDiseasesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdatePatientDiseasesCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UpdatePatientDiseasesCommand request, CancellationToken cancellationToken)
    {
        var patient = await context.Patients.Include(x => x.Diseases)
            .FirstOrDefaultAsync(x => x.Id == request.PatientId && x.WorkspaceId == request.WorkspaceId,
                cancellationToken);

        if (patient == null)
        {
            return Error.NotFound(description: "Patient not found.");
        }

        var patientDiseases = request.DiseaseIds.Select(x => new PatientDisease
        {
            DiseaseId = x,
            PatientId = request.PatientId
        }).ToList();

        patient.Diseases = patientDiseases;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}