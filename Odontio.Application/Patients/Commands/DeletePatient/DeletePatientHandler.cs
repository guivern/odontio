﻿using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Patients.Commands.DeletePatient;

public class DeletePatientHandler(IApplicationDbContext context) : IRequestHandler<DeletePatientCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await context.Patients
            .Where(x => x.Id == request.Id)
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (patient == null)
        {
            return Error.NotFound(description: "Patient not found");
        }

        context.Patients.Remove(patient);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            return Error.Conflict(description: "Can not delete patient due to existing dependencies.");
        }

        return Unit.Value;
    }
}