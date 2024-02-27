using System.Text.Json.Serialization;

namespace Odontio.Application.Common.Interfaces;

public interface IPatientResource: IWorkspaceResource
{
    public long PatientId { get; }
}