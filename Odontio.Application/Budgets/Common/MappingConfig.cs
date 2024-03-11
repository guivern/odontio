using Odontio.Domain.Entities;

namespace Odontio.Application.Budgets.Common;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Budget, GetBudgetResult>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}")
            .Map(dest => dest.TotalCost, src => src.PatientTreatments.Sum(x => x.Cost));
    }
}