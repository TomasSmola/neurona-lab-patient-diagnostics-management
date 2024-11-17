using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Services;

public interface IPatientService
{
    Task<IEnumerable<Patient>> GetPatientsAsync(CancellationToken cancellationToken);
    Task<Patient?> GetPatientByIdAsync(int id, CancellationToken cancellationToken);
    Task<Patient> CreatePatientAsync(string name, int age, CancellationToken cancellationToken);
    Task<Diagnosis> AddDiagnosisAsync(int patientId, string result, CancellationToken cancellationToken);
}
