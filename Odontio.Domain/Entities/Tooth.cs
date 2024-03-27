using Odontio.Domain.Enums;

namespace Odontio.Domain.Entities;

public class Tooth
{
    public long Id {get; set;}
    public string Name {get; set;}
    public int Number {get; set;}
    public OdontogramType OdontogramType {get; set;}
    public ToothType Type {get; set;}
}