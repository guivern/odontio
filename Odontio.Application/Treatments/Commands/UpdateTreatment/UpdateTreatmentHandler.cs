using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Commands.UpdateTreatment;

public class UpdateTreatmentHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<UpdateTreatmentCommand, ErrorOr<UpdateTreatmentResult>>
{
    public async Task<ErrorOr<UpdateTreatmentResult>> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await context.Treatments.FindAsync(request.Id);

        if (treatment is null)
        {
            return Error.NotFound(description: "Treatment not found");
        }

        mapper.Map(request, treatment);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The treatment was modified by another user");
        }

        var result = mapper.Map<UpdateTreatmentResult>(treatment);

        return result;
    }
}