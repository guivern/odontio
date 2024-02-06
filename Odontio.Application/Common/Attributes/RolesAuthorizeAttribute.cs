namespace Odontio.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RolesAuthorizeAttribute : Attribute
{
    public string[] Roles { get; }

    public RolesAuthorizeAttribute(params string[] roles)
    {
        Roles = roles;
    }
}