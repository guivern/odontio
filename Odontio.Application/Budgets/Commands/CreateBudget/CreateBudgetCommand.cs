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
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    
    public List<CreatePatientTreatmentDto> PatientTreatments { get; set; } = new();
}

public class CreatePatientTreatmentDto
{
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}