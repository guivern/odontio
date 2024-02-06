using Odontio.Application.Treatments.Queries.GetTreatments;
using Odontio.Domain.Entities;

namespace Odontio.Application.Treatments;

public class MappingConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    { 
        config.NewConfig<Treatment, GetTreatmentsResult>()
            .Map(dest => dest.CategoryName, src => src.Category.Name);
    }
}