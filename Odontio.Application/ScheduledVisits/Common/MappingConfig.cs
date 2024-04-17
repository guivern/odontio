using Odontio.Domain.Entities;

namespace Odontio.Application.ScheduledVisits.Common;

public class MappingConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ScheduledVisit, GetScheduledVisitResult>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}");
    }
}