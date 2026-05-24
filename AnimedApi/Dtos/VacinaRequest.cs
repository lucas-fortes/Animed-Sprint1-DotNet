namespace AnimedApi.Dtos;

public class VacinaRequest
{
    public string Nome { get; set; } = string.Empty;

    public DateTime DataAplicacao { get; set; }

    public DateTime? DataProximaDose { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public int PetId { get; set; }
}