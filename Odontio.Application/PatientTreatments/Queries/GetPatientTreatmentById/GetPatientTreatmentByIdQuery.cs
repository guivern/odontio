using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatmentById;

public class GetPatientTreatmentByIdQuery : IRequest<ErrorOr<GetPatientTreatmentFullResult>>, IPatientResource
{
    public long Id { get; set; }
    public long BudgetId { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}