namespace Odontio.API.Contracts.PatientDiseases;

public class AddPatientDiseasesRequest
{
    public List<long> DiseaseIds { get; set; } = new();
}