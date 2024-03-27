using Odontio.Application.Common.Interfaces;
using Odontio.Application.Common.ViewModels;

namespace Odontio.Application.Patients.Queries.GetMedicalRecordPdf;

public class GetMedicalRecordPdfHandler(IApplicationDbContext context, IPdfRenderer pdfRenderer, IMapper mapper) : IRequestHandler<GetMedicalRecordPdfQuery, ErrorOr<byte[]>>
{
    public async Task<ErrorOr<byte[]>> Handle(GetMedicalRecordPdfQuery request, CancellationToken cancellationToken)
    {
        var patient = await context.Patients
            .AsNoTracking()
            .Include(x => x.Referred)
            .Include(x => x.MedicalConditions)
            .Include(x => x.Diseases)
            .Where(p => p.Id == request.Id && p.WorkspaceId == request.WorkspaceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (patient == null)
        {
            return Error.NotFound(description: "Patient not found");
        }

        var vmodel = mapper.Map<MedicalRecordViewModel>(patient);
        var diseases = await context.Diseases.AsNoTracking().ToListAsync(cancellationToken);
        var diseasesViewModel = new List<PatientDiseaseViewModel>();
        
        
        foreach (var disease in diseases)
        {
            var patientDisease = patient.Diseases.FirstOrDefault(x => x.DiseaseId == disease.Id);
            var diseaseViewModel = new PatientDiseaseViewModel
            {
                Name = disease.Name,
                HasCondition = patientDisease != null
            };
            
            vmodel.Diseases.Add(diseaseViewModel);
        }
        
        var testPdfFilt = await pdfRenderer.RenderAsync("MedicalRecordView", vmodel);
        
        return testPdfFilt;
    }
}