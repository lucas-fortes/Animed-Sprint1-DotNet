namespace AnimedApi.Dtos;

public class ConsultaResponse
{
    public int Id { get; set; }

    public DateTime DataConsulta { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public string Diagnostico { get; set; } = string.Empty;

    public string Tratamento { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public string NivelUrgencia { get; set; } = string.Empty;

    public int TutorId { get; set; }

    public string NomeTutor { get; set; } = string.Empty;

    public int PetId { get; set; }

    public string NomePet { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}