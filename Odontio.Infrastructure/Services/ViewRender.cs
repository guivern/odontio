using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Odontio.Infrastructure.Services;

public class ViewRender(
    IRazorViewEngine  viewEngine,
    ITempDataProvider tempDataProvider,
    IServiceProvider serviceProvider) : IViewRender
{
    public async Task<string> RenderAsync<T>(string name, T model)
    {
        var actionContext = GetActionContext();

        var viewEngineResult = viewEngine.FindView(actionContext, name, false);

        if (!viewEngineResult.Success)
        {
            throw new InvalidOperationException($"Couldn't find view '{name}'");
        }

        var view = viewEngineResult.View;

        using (var output = new StringWriter())
        {
            var viewContext = new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<T>(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(
                    actionContext.HttpContext,
                    tempDataProvider),
                output,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }
    }

    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}