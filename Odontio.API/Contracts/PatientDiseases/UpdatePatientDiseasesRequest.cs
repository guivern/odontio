namespace Odontio.API.Contracts.PatientDiseases;

public class UpdatePatientDiseasesRequest
{
    public List<long> DiseaseIds { get; set; } = new();
}