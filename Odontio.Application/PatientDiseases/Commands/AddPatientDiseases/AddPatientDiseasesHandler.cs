using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.PatientDiseases.Commands.AddPatientDiseases;

public class AddPatientDiseasesHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<AddPatientDiseasesCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AddPatientDiseasesCommand request, CancellationToken cancellationToken)
    {
        var patientDiseases = request.DiseaseIds.Select(x => new PatientDisease
        {
            DiseaseId = x,
            PatientId = request.PatientId
        });

        await context.PatientDiseases.AddRangeAsync(patientDiseases, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}