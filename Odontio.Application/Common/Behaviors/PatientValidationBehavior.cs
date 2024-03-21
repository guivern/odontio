using System.Net;
using Microsoft.AspNetCore.Http;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Common.Behaviors;

public class PatientValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest: IPatientResource
{
    private readonly IApplicationDbContext _context;

    public PatientValidationBehavior(IApplicationDbContext context)
    {
        _context = context;
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
                .Where(x => x.WorkspaceId == request.WorkspaceId)
                .FirstOrDefaultAsync(x => x.Id == patientId, cancellationToken);
            
            if (patient == null)
                return (dynamic)Error.Custom((int)ErrorType.NotFound, "NOT_FOUND", "Patient not found");
        }

        return await next();
    }
}