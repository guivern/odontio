using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatmentById;

public class GetPatientTreatmentByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPatientTreatmentByIdQuery, ErrorOr<GetPatientTreatmentFullResult>>
{
    public async Task<ErrorOr<GetPatientTreatmentFullResult>> Handle(GetPatientTreatmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var patientTreatment = await context.PatientTreatments
            .AsNoTracking()
            .Include(x => x.Budget)
            .ThenInclude(x => x.Patient)
            .Include(x => x.Treatment)
            .Include(x => x.Tooth)
            .Include(x => x.MedicaNotes)
            .ThenInclude(x => x.Appointment)
            .Where(x => x.BudgetId == request.BudgetId)
            .Where(x => x.Budget.PatientId == request.PatientId)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (patientTreatment == null)
        {
            return Error.NotFound(description: "Patient treatment not found");
        }

        var result = mapper.Map<GetPatientTreatmentFullResult>(patientTreatment);

        return result;
    }
}