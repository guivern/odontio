using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.PatientTreatments.Commands.CreatePatientTreatment;

public class CreatePatientTreatmentHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CreatePatientTreatmentCommand, ErrorOr<UpsertPatientTreatmentResult>>
{
    public async Task<ErrorOr<UpsertPatientTreatmentResult>> Handle(CreatePatientTreatmentCommand request, CancellationToken cancellationToken)
    {
        var budget = await context.Budgets
            .Include(b => b.PatientTreatments)
            .FirstOrDefaultAsync(b => b.Id == request.BudgetId, cancellationToken);

        if (budget == null)
        {
            return Error.NotFound(description: "Budget not found");
        }

        var patientTreatment = mapper.Map<PatientTreatment>(request);

        patientTreatment.Status = TreatmentStatus.Pending;

        budget.PatientTreatments.Add(patientTreatment);

        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<UpsertPatientTreatmentResult>(patientTreatment);
        
        return result;
    }
}