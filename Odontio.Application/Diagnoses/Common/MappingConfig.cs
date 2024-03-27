using Odontio.Domain.Entities;

namespace Odontio.Application.Diagnoses.Common;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Diagnosis, UpsertDiagnosisResult>()
            .Map(dest => dest.ToothName, src => src.Tooth.Name)
            .Map(dest => dest.ToothType, src => src.Tooth.Type)
            .Map(dest => dest.Odontogram, src => src.Tooth.OdontogramType);
    }
}