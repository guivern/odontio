﻿using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class ScheduledVisit : BaseAuditableEntity
{
    public long Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Description { get; set; }

    public long PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}