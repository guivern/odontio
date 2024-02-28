using System.Net;
using Microsoft.AspNetCore.Http;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Common.Behaviors;

public class PatientValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest: IPatientResource
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PatientValidationBehavior(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var attributeExists = Attribute.IsDefined(requestType, typeof(ValidatePatientAttribute));
        if (attributeExists)
        {
            // check if patient exists
            var patientId = request.PatientId;
            var patient = await _context.Patients.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == patientId, cancellationToken);
            
            if (patient == null)
                return (dynamic)Error.Custom((int)HttpStatusCode.NotFound, "NOT_FOUND", "Patient not found");
            
            // check if patient belongs to workspace
            var workspaceId = request.WorkspaceId;
            if (patient.WorkspaceId != workspaceId)
                return (dynamic)Error.Custom((int)HttpStatusCode.BadRequest, "BAD_REQUEST", "Patient does not belong to workspace");
        }

        return await next();
    }
}