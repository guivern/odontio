using System.ComponentModel.DataAnnotations;

namespace Odontio.Domain.Enums;

public enum ElisaTestStatus
{
    [Display(Name = "Se hizo")]
    SeHizo = 1,
    [Display(Name = "No se hizo")]
    NoSeHizo,
    [Display(Name = "No sabe")]
    NoSabe
}