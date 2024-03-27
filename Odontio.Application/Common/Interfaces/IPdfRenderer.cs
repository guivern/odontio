using Microsoft.AspNetCore.Mvc;

namespace Odontio.Application.Common.Interfaces;

public interface IPdfRenderer
{
    Task<byte[]> RenderAsync<T>(string viewName, T model);
}