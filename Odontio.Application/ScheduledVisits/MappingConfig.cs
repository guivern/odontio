using Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;
using Odontio.Domain.Entities;

namespace Odontio.Application.ScheduledVisits;

public class MappingConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ScheduledVisit, GetScheduledVisitByWorkspaceResult>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}");
    }
}