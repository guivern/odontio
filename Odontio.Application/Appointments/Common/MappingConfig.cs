using Odontio.Domain.Entities;

namespace Odontio.Application.Appointments.Common;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Appointment, GetAppointmentResult>()
            .Map(dest => dest.PatientName, src => $"{src.Patient.FirstName} {src.Patient.LastName}");

        config.NewConfig<MedicalRecord, GetMedicalRecordResult>()
            .Map(dest => dest.TreatmentName, src => src.PatientTreatment.Treatment.Name);
    }
}