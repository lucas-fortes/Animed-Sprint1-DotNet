namespace AnimedApi.Dtos;

public class VacinaResponse
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public DateTime DataAplicacao { get; set; }

    public DateTime? DataProximaDose { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public int PetId { get; set; }

    public string NomePet { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}