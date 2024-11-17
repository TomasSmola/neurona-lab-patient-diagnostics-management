namespace NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

public class Diagnosis
{
    public int Id { get; set; }
    public string Result { get; set; } = null!;
    public DateTimeOffset Date { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
