using Odontio.Domain.Entities;

namespace Odontio.Application.MedicalNotes.Common;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Entities.MedicalNote, GetMedicalNoteFullResult>()
            .Map(dest => dest.TreatmentName, src => src.PatientTreatment.Treatment.Name)
            .Map(dest => dest.PatientName,
                src =>
                    $"{src.PatientTreatment.Budget.Patient.FirstName} {src.PatientTreatment.Budget.Patient.LastName}")
            .Map(dest => dest.Date, src => src.Appointment.Date)
            .Map(dest => dest.BudgetId, src => src.PatientTreatment.BudgetId)
            .Map(dest => dest.PatientTreatmentId, src => src.PatientTreatment.Id);
    }
}