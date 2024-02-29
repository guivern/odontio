﻿using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;

namespace Odontio.Application.Diagnoses.Queries.GetDiagnosisById;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetDiagnosisByIdQuery: IRequest<ErrorOr<UpsertDiagnosisResult>>, IPatientResource
{
    public long Id { get; set; }
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}