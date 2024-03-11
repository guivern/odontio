using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;

namespace Odontio.Application.PatientTreatments.Commands.UpdatePatientTreatment;

public class UpdatePatientTreatmentHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdatePatientTreatmentCommand, ErrorOr<UpsertPatientTreatmentResult>>
{
    public async Task<ErrorOr<UpsertPatientTreatmentResult>> Handle(UpdatePatientTreatmentCommand request, CancellationToken cancellationToken)
    {
        var patientTreatment = await context.PatientTreatments.FirstAsync(x => x.Id == request.Id, cancellationToken);

        patientTreatment = mapper.Map(request, patientTreatment);
        
        context.PatientTreatments.Entry(patientTreatment).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The patient treatment was modified by another user");
        }

        var result = mapper.Map<UpsertPatientTreatmentResult>(patientTreatment);
        
        return result;
    }
}