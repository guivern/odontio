using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetTreatmentStatus : IRequest<List<EnumValue>>
{
}

public class GetTreatmentStatusHandler : IRequestHandler<GetTreatmentStatus, List<EnumValue>>
{
    public Task<List<EnumValue>> Handle(GetTreatmentStatus request, CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>
        {
            new(nameof(TreatmentStatus.Pending), "Pendiente"),
            new(nameof(TreatmentStatus.InProgress), "En progreso"),
            new(nameof(TreatmentStatus.Finished), "Finalizado"),
        };

        return Task.FromResult(result);
    }
}