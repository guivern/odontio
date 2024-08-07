using Odontio.Application.Budgets.Commands.CreateBudget;
using Odontio.Application.Budgets.Commands.UpdateBudget;
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
            .Map(dest => dest.Balance, src => src.PatientTreatments.Sum(x => x.Cost) - src.Payments.Sum(x => x.Amount))
            .Map(dest => dest.Details, src => src.PatientTreatments);

        config.NewConfig<Diagnosis, BudgetDiagnosisDto>();
        config.NewConfig<Treatment, BudgetTreatmentDto>();
        
        config.NewConfig<Budget, GetBudgetResult>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}")
            .Map(dest => dest.TotalCost, src => src.PatientTreatments.Sum(x => x.Cost))
            .Map(dest => dest.TotalPayments, src => src.Payments.Sum(x => x.Amount))
            .Map(dest => dest.Balance, src => src.PatientTreatments.Sum(x => x.Cost) - src.Payments.Sum(x => x.Amount));

        config.NewConfig<CreateBudgetCommand, Budget>()
            .Map(dest => dest.PatientTreatments, src => src.Details.Select(x => new PatientTreatment
            {
                TreatmentId = x.Treatment.Id,
                Observations = x.Observations,
                Cost = x.Cost
            }).ToList())
            .Ignore(x => x.PatientTreatments);
        
        config.NewConfig<UpdateBudgetCommand, Budget>()
            .Map(dest => dest.PatientTreatments, src => src.Details.Select(x => new PatientTreatment
            {
                TreatmentId = x.Treatment.Id,
                Observations = x.Observations,
                Cost = x.Cost
            }).ToList())
            .Ignore(x => x.PatientTreatments);
    }
}