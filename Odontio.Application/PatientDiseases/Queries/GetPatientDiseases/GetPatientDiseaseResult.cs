namespace Odontio.Application.PatientDiseases.Queries.GetPatientDiseases;

public class GetPatientDiseaseResult
{
    public long Id { get; set; }
    public long PatientId { get; set; }
    public long DiseaseId { get; set; }
    public string DiseaseName { get; set; } = string.Empty;
}