using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Data.Repositories;

public interface IPatientRepository
{
    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken);
    Task<Diagnosis> AddAsync(Diagnosis diagnosis, CancellationToken cancellationToken);
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken);
}
