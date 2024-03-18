using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Payments.Common;

namespace Odontio.Application.Payments.Commands.UpdatePayment;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class UpdatePaymentCommand : IRequest<ErrorOr<UpsertPaymentResult>>, IPatientResource
{
    public long Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string ReceiptType { get; set; }
    public string? ReceiptNumber { get; set; }
    public long BudgetId { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}