using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Commands.AddPatientDiseases;

public class AddPatientDiseasesValidator : AbstractValidator<AddPatientDiseasesCommand>
{
    private readonly IApplicationDbContext _context;

    public AddPatientDiseasesValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.DiseaseIds).NotEmpty();

        // validata each diseaseId is unique for the patient
        RuleForEach(x => x.DiseaseIds)
            .SetAsyncValidator(new CreatePatientDiseaseValidator(_context));
        
        // validate repeated diseaseIds
        RuleFor(x => x.DiseaseIds)
            .Must(x => x.Count == x.Distinct().Count())
            .WithMessage("Repeated disease ids.");
    }
}

public class CreatePatientDiseaseValidator(IApplicationDbContext dbContext)
    : IAsyncPropertyValidator<AddPatientDiseasesCommand, long>
{
    public async Task<bool> IsValidAsync(ValidationContext<AddPatientDiseasesCommand> context,
        long value, CancellationToken cancellation)
    {
        // validata each diseaseId is unique for the patient
        var exists = await dbContext.PatientDiseases
            .AnyAsync(x => x.DiseaseId == value && x.PatientId == context.InstanceToValidate.PatientId, cancellation);


        if (exists)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage",
                $"Disease {value} already exists for this patient.");
        }

        return !exists;
    }

    public string Name => nameof(CreatePatientDiseaseValidator);

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "{ErrorMessage}";
    }
}