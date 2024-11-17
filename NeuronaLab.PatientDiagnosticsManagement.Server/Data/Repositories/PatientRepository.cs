using Microsoft.EntityFrameworkCore;
using NeuronaLab.PatientDiagnosticsManagement.Server.Data.Contexts;
using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;
using System.Threading;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Data.Repositories;

public class PatientRepository(IDbContextFactory<AppDbContext> dbContextFactory) : IPatientRepository
{
    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = dbContextFactory.CreateDbContext();
        return await dbContext.Patients
            .Include(p => p.Diagnoses)
            .Select(p => new Patient
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Diagnoses = p.Diagnoses.OrderByDescending(d => d.Date).Take(1).ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = dbContextFactory.CreateDbContext();
        return await dbContext.Patients
            .Include(p => p.Diagnoses)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        await using var dbContext = dbContextFactory.CreateDbContext();
        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync(cancellationToken);
        return patient;
    }

    public async Task<Diagnosis> AddAsync(Diagnosis diagnosis, CancellationToken cancellationToken)
    {
        await using var dbContext = dbContextFactory.CreateDbContext();
        dbContext.Diagnoses.Add(diagnosis);
        await dbContext.SaveChangesAsync(cancellationToken);
        return diagnosis;
    }
}
