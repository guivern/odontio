namespace Odontio.Application.Common.Exceptions;

public class UnauthorizedException: Exception
{
    public UnauthorizedException() : base("User is not authorized to access this resource.")
    {
    }
}