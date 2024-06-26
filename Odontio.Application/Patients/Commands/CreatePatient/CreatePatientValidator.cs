﻿using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Patients.Commands.CreatePatient;

public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    private readonly IApplicationDbContext _context;

    public CreatePatientValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.WorkspaceId).NotEmpty().WithMessage("Workspace id is required");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.BusinessName).MaximumLength(200);
        RuleFor(x => x.Ruc).MaximumLength(20);
        RuleFor(x => x.Address).MaximumLength(256);
        RuleFor(x => x.WorkAddress).MaximumLength(256);
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress();
        RuleFor(x => x.Phone).MaximumLength(20);
        RuleFor(x => x.WorkPhone).MaximumLength(20);
        RuleFor(x => x.Observations).MaximumLength(256);
        RuleFor(x => x.Occupation).MaximumLength(48);
        RuleFor(x => x.LastDentalVisit).MaximumLength(100);
        RuleFor(x => x.ToothLossCause).MaximumLength(100);
        RuleFor(x => x.Gender).NotEmpty().Must(BeValidGender).WithMessage("Genero inválido");
        RuleFor(x => x.MaritalStatus).Must(BeValidMaritalStatus).WithMessage("Estado civil inválido");
        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(BeUniqueDocumentNumber)
            .WithMessage(x => $"Ya existe un paciente con este número de documento");
        
        // RuleFor(x => x.MedicalConditions)
        //     .ForEach(x =>
        //         x.Must(mc => !string.IsNullOrEmpty(mc.ConditionType)).WithMessage("Is required"));
    }

    private bool BeValidMaritalStatus(string? arg)
    {
        var result = Enum.TryParse<MaritalStatus>(arg, out _);
        return string.IsNullOrEmpty(arg) || Enum.TryParse<MaritalStatus>(arg, out _);
    }

    private bool BeValidGender(string arg1)
    {
        return Enum.TryParse<Gender>(arg1, out _);
    }

    private async Task<bool> BeUniqueDocumentNumber(CreatePatientCommand arg1, string arg2,  CancellationToken arg3) 
    {
        // validate if exists a patient with the same document number in the same workspace
        return !await _context.Patients
            .AsNoTracking()
            .Where(x => x.DocumentNumber.ToLower() == arg2.ToLower() && x.WorkspaceId == arg1.WorkspaceId)
            .AnyAsync(arg3);
    }
}