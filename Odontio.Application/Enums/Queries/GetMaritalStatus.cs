using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetMaritalStatus : IRequest<ErrorOr<List<string>>>
{
    public string Gender { get; set; }
}

public class GetMaritalStatusHandler : IRequestHandler<GetMaritalStatus, ErrorOr<List<string>>>
{
    public async Task<ErrorOr<List<string>>> Handle(GetMaritalStatus request,
        CancellationToken cancellationToken)
    {
        var result = new List<string>();
        
        var validGenders = new[] { "Masculino", "Femenino" };
        if (!validGenders.Contains(request.Gender))
        {
            return Error.Validation(description: "Gender is invalid. Valid values are: Male, Female.");
        }

        var maritalStatus =  Enum.GetNames(typeof(MaritalStatus)).ToList();

        if (request.Gender == nameof(Gender.Masculino))
        {
            // from 0 to 3
            result = maritalStatus.GetRange(0, 4);
        }
        else
        {
            // from 4 to 7
            result = maritalStatus.GetRange(4, 4);
        }

        return result;
    }
}