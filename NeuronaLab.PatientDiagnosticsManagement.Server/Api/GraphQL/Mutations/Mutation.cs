using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;
using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Services;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Api.GraphQL.Mutations;

public class Mutation(IPatientService patientService)
{
    public async Task<Patient> AddPatient(string name, int age, CancellationToken cancellationToken) =>
        await patientService.CreatePatientAsync(name, age, cancellationToken);

    public async Task<Diagnosis> AddDiagnosis(int patientId, string result, CancellationToken cancellationToken) =>
        await patientService.AddDiagnosisAsync(patientId, result, cancellationToken);
}
