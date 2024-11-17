namespace NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public ICollection<Diagnosis> Diagnoses { get; set; } = [];
}
