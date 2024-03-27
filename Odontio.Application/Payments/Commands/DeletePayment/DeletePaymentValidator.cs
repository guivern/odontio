namespace Odontio.Application.Payments.Commands.DeletePayment;

public class DeletePaymentValidator : AbstractValidator<DeletePaymentCommand>
{
    public DeletePaymentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}