using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Payments.Common;

namespace Odontio.Application.Payments.Queries.GetPayments;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetPaymentsQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<UpsertPaymentResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long? PatientId { get; set; }
}