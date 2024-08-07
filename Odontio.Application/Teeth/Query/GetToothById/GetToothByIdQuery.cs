
using Odontio.Application.Teeth.Query.Common;

namespace Odontio.Application.Teeth.Query.GetToothById;

public class GetToothByIdQuery: IRequest<ErrorOr<GetToothResult>>
{
    public long Id { get; set; }
}