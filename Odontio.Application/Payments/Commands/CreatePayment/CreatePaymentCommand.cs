using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Payments.Common;

namespace Odontio.Application.Payments.Commands.CreatePayment;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class CreatePaymentCommand : IRequest<ErrorOr<UpsertPaymentResult>>, IPatientResource
{
    public DateTimeOffset? Date { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string ReceiptType { get; set; }
    public string? ReceiptNumber { get; set; }
    public long BudgetId { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}