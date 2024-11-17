using Microsoft.EntityFrameworkCore;
using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
}
