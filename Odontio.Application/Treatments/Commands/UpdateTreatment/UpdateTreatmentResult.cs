﻿namespace Odontio.Application.Treatments.Commands.UpdateTreatment;

public class UpdateTreatmentResult
{
    public long Id {get; set;}
    public string Name {get; set;} = null!;
    public string? Description {get; set;}
    public decimal? Cost {get; set;}
    public long WorkspaceId {get; set;}
    public long CategoryId {get; set;}
}