using Microsoft.AspNetCore.Mvc;

namespace Odontio.Application.Common.Interfaces;

public interface IWorkspaceResource
{
    public long WorkspaceId { get; }
}