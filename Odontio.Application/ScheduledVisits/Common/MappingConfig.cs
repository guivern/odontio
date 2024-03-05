using Odontio.Domain.Entities;

namespace Odontio.Application.ScheduledVisits.Common;

public class MappingConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ScheduledVisit, UpsertScheduledVisitResult>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}");
    }
}