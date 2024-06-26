﻿using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetScheduledVisitsByWorkspaceQuery: IRequest<ErrorOr<List<GetScheduledVisitResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public DateRange DateRange { get; set; }
}