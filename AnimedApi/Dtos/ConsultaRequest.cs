namespace AnimedApi.Dtos;

public class ConsultaRequest
{
    public DateTime DataConsulta { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public string Diagnostico { get; set; } = string.Empty;

    public string Tratamento { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public string NivelUrgencia { get; set; } = string.Empty;

    public int TutorId { get; set; }

    public int PetId { get; set; }
}