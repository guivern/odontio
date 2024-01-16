using System.ComponentModel.DataAnnotations;

namespace Odontio.Domain.Enums;

public enum PregnancyStatus
{
    [Display(Name = "Yes")]
    Embarazada = 1,
    [Display(Name = "No")]
    NoEmbarazada,
    [Display(Name = "Unknown")]
    NoSabe
}