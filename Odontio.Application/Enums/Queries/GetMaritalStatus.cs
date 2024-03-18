using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetMaritalStatus : IRequest<ErrorOr<List<EnumValue>>>
{
    public string Gender { get; set; }
}

public class GetMaritalStatusHandler : IRequestHandler<GetMaritalStatus, ErrorOr<List<EnumValue>>>
{
    public async Task<ErrorOr<List<EnumValue>>> Handle(GetMaritalStatus request,
        CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>();
        
        var validGenders = new[] { "Male", "Female" };
        if (!validGenders.Contains(request.Gender))
        {
            return Error.Validation(description: "Gender is invalid. Valid values are: Male, Female.");
        }

        if (request.Gender == "Male")
        {
            result.Add(new(nameof(MaritalStatus.Single), "Soltero"));
            result.Add(new(nameof(MaritalStatus.Married), "Casado"));
            result.Add(new(nameof(MaritalStatus.Divorced), "Divorciado"));
            result.Add(new(nameof(MaritalStatus.Widowed), "Viudo"));
        }
        else
        {
            result.Add(new(nameof(MaritalStatus.Single), "Soltera"));
            result.Add(new(nameof(MaritalStatus.Married), "Casada"));
            result.Add(new(nameof(MaritalStatus.Divorced), "Divorciada"));
            result.Add(new(nameof(MaritalStatus.Widowed), "Viuda"));
        }

        return result;
    }
}