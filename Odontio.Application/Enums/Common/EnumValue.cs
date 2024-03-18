namespace Odontio.Application.Enums.Common;

public class EnumValue
{
    public EnumValue(string value, string display)
    {
        Value = value;
        Display = display;
    }

    public string Value { get; set; }
    public string Display { get; set; }
}