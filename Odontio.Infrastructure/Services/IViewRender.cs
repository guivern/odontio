namespace Odontio.Infrastructure.Services;

public interface IViewRender
{
    Task<string> RenderAsync<T>(string name, T model);
}