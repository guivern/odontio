using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odontio.Domain.Enums;

public enum TreatmentStatus
{
    Pending = 1,
    InProgress,
    Finished
}