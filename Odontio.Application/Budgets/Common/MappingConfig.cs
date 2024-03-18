using Odontio.Domain.Entities;

namespace Odontio.Application.Budgets.Common;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Budget, GetBudgetFullResult>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}")
            .Map(dest => dest.TotalCost, src => src.PatientTreatments.Sum(x => x.Cost))
            .Map(dest => dest.TotalPayments, src => src.Payments.Sum(x => x.Amount))
            .Map(dest => dest.Balance, src => src.PatientTreatments.Sum(x => x.Cost) - src.Payments.Sum(x => x.Amount));
        
        config.NewConfig<Budget, GetBudgetResultDto>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}")
            .Map(dest => dest.TotalCost, src => src.PatientTreatments.Sum(x => x.Cost))
            .Map(dest => dest.TotalPayments, src => src.Payments.Sum(x => x.Amount))
            .Map(dest => dest.Balance, src => src.PatientTreatments.Sum(x => x.Cost) - src.Payments.Sum(x => x.Amount));
            
    }
}