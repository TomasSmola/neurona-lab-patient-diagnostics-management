using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;
using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Services;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Api.GraphQL.Queries;

public class Query(IPatientService patientService)
{
    public async Task<IEnumerable<Patient>> GetPatients(CancellationToken cancellationToken) =>
        await patientService.GetPatientsAsync(cancellationToken);

    public async Task<Patient?> GetPatient(int id, CancellationToken cancellationToken) =>
        await patientService.GetPatientByIdAsync(id, cancellationToken);
}
