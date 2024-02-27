using Odontio.Application.Patients.Commands.CreatePatient;
using Odontio.Application.Patients.Common;
using Odontio.Application.Patients.Queries.GetPatientById;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

namespace Odontio.Application.Patients;

public class MappginConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Patient, GetPatientByIdResult>()
            .Map(dest => dest.Gender, src => src.Gender.ToString())
            .Map(dest => dest.MaritalStatus, src => src.MaritalStatus.ToString());

        config.NewConfig<Patient, UpsertPatientResult>()
            .Map(dest => dest.Gender, src => src.Gender.ToString())
            .Map(dest => dest.MaritalStatus, src => src.MaritalStatus.ToString());

        config.NewConfig<CreatePatientCommand, Patient>()
            .Map(dest => dest.Gender, src => (Gender)Enum.Parse(typeof(Gender), src.Gender))
            .Map(dest => dest.MaritalStatus,
                src => src.MaritalStatus != null
                    ? (MaritalStatus)Enum.Parse(typeof(MaritalStatus), src.MaritalStatus)
                    : (MaritalStatus?)null);
    }
}