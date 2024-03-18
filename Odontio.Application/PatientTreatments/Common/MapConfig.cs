using Odontio.Application.PatientTreatments.Commands.UpdatePatientTreatment;
using Odontio.Domain.Entities;

namespace Odontio.Application.PatientTreatments.Common;

public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UpdatePatientTreatmentCommand, PatientTreatment>()
            .Map(dest => dest.Status, src => src.Status);
        config.NewConfig<PatientTreatment, GetPatientTreatmentFullResult>()
            .Map(dest => dest.PatientName, src => $"{src.Budget.Patient.FirstName} {src.Budget.Patient.LastName}")
            .Map(dest => dest.PatientId, src => src.Budget.PatientId);
        config.NewConfig<MedicalRecord, GetMedicalRecordResultDto>()
            .Map(dest => dest.Date, src => src.Appointment.Date);
        config.NewConfig<PatientTreatment, GetPatientTreatmentResult>()
            .Map(dest => dest.PatientName, src => $"{src.Budget.Patient.FirstName} {src.Budget.Patient.LastName}")
            .Map(dest => dest.PatientId, src => src.Budget.PatientId);
    }
}