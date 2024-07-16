﻿using Odontio.Application.Budgets.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.CreateBudget;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class CreateBudgetCommand: IRequest<ErrorOr<UpsertBudgetResult>>, IPatientResource
{
    public DateOnly? Date { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string? Observations { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    
    public List<CreatePatientTreatmentDto> PatientTreatments { get; set; } = new();
}

public class CreatePatientTreatmentDto
{
    public long TreatmentId { get; set; }
    public long? DiagnosisId { get; set; }
    public decimal Cost { get; set; }
}