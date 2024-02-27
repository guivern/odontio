using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Odontio.Application.Common.Interfaces;

public interface IWorkspaceResource
{
    public long WorkspaceId { get; }
}