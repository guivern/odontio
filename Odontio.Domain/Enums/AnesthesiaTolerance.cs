using System.ComponentModel.DataAnnotations;

namespace Odontio.Domain.Enums;

public enum AnesthesiaTolerance
{
    [Display(Name = "Yes")]
    Si = 1,
    [Display(Name = "No")]
    No,
    [Display(Name = "Unknown")]
    NuncaTomo
}