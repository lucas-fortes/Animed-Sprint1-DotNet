namespace AnimedApi.Models;

public class Consulta
{
    public int Id { get; set; }

    public DateTime DataConsulta { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public string Diagnostico { get; set; } = string.Empty;

    public string Tratamento { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public string NivelUrgencia { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public int TutorId { get; set; }

    public Tutor? Tutor { get; set; }

    public int PetId { get; set; }

    public Pet? Pet { get; set; }
}