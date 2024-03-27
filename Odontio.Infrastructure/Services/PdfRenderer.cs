using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Odontio.Application.Common.Interfaces;
using WkWrap.Core;

namespace Odontio.Infrastructure.Services;

public class PdfRenderer(
    IConfiguration configuration,
    IRazorViewEngine viewEngine,
    ITempDataProvider tempDataProvider,
    IServiceProvider serviceProvider)
    : IPdfRenderer
{
    public async Task<byte[]> RenderAsync<T>(string viewName, T model)
    {
        var wkHtmlToPdfPath = configuration["WkhtmlToPdf:Path"];
        var html = await RenderViewAsync(viewName, model);
        var wkhtmltopdf = new FileInfo(wkHtmlToPdfPath);
        var converter = new HtmlToPdfConverter(wkhtmltopdf);
        
        var result = converter.ConvertToPdf(html);

        return result;
    }
    
    private async Task<string> RenderViewAsync<T>(string name, T model)
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