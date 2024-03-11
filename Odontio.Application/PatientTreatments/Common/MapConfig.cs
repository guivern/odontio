using Odontio.Application.PatientTreatments.Commands.UpdatePatientTreatment;
using Odontio.Domain.Entities;

namespace Odontio.Application.PatientTreatments.Common;

public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UpdatePatientTreatmentCommand, PatientTreatment>()
            .Map(dest => dest.Status, src => src.Status);
    }
}