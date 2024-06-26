﻿using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.Common;

namespace Odontio.Application.MedicalConditions.Queries.GetMedicalConditions;

public class GetMedicalConditionsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMedicalConditionsQuery, ErrorOr<List<MedicalConditionResult>>>
{
    public async Task<ErrorOr<List<MedicalConditionResult>>> Handle(GetMedicalConditionsQuery request,
        CancellationToken cancellationToken)
    {
        var medicalConditions = await context.MedicalConditions
            .AsNoTracking()
            .Where(x => x.PatientId == request.PatientId)
            .ProjectToType<MedicalConditionResult>()
            .ToListAsync(cancellationToken);

        return medicalConditions;
    }
}