using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odontio.Domain.Enums;

public enum TreatmentStatus
{
    [Display(Name = "Pendiente")]
    Pending = 1,
    [Display(Name = "En curso")]
    InProgress,
    [Display(Name = "Finalizado")]
    Finished
}