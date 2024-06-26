﻿using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientDiseases.Commands.UpdatePatientDiseases;

public class UpdatePatientDiseasesValidator : AbstractValidator<UpdatePatientDiseasesCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePatientDiseasesValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.DiseaseIds).NotEmpty();

        // validate each diseaseId exists in the workspace
        RuleForEach(x => x.DiseaseIds)
            .MustAsync(async (diseaseId, cancellation) =>
            {
                return await _context.Diseases
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == diseaseId && x.WorkspaceId == x.WorkspaceId, cancellation);
            })
            .WithMessage("Disease not found in the workspace.");

        // validate repeated diseaseIds
        RuleFor(x => x.DiseaseIds)
            .Must(x => x.Count == x.Distinct().Count())
            .WithMessage("Repeated disease ids.");
    }
}

public class CreatePatientDiseaseValidator(IApplicationDbContext dbContext)
    : IAsyncPropertyValidator<UpdatePatientDiseasesCommand, long>
{
    public async Task<bool> IsValidAsync(ValidationContext<UpdatePatientDiseasesCommand> context,
        long value, CancellationToken cancellation)
    {
        // validata each diseaseId is unique for the patient
        var exists = await dbContext.PatientDiseases
            .AsNoTracking()
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