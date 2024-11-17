using NeuronaLab.PatientDiagnosticsManagement.Server.Data.Repositories;
using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Services;

public class PatientService(IPatientRepository patientRepository) : IPatientService
{
    public async Task<IEnumerable<Patient>> GetPatientsAsync(CancellationToken cancellationToken) =>
        await patientRepository.GetAllAsync(cancellationToken);

    public async Task<Patient?> GetPatientByIdAsync(int id, CancellationToken cancellationToken) =>
        await patientRepository.GetByIdAsync(id, cancellationToken);

    public async Task<Patient> CreatePatientAsync(string name, int age, CancellationToken cancellationToken)
    {
        var patient = new Patient { Name = name, Age = age };
        return await patientRepository.AddAsync(patient, cancellationToken);
    }

    public async Task<Diagnosis> AddDiagnosisAsync(int patientId, string result, CancellationToken cancellationToken)
    {
        var patient = await GetPatientByIdAsync(patientId, cancellationToken)
            ?? throw new ArgumentException("Patient not found", nameof(patientId));

        var diagnosis = new Diagnosis { Result = result, PatientId = patientId, Date = DateTimeOffset.Now };
        return await patientRepository.AddAsync(diagnosis, cancellationToken);
    }
}
