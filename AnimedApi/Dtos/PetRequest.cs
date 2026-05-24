namespace AnimedApi.Dtos;

public class PetRequest
{
    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string Raca { get; set; } = string.Empty;

    public int Idade { get; set; }

    public decimal Peso { get; set; }

    public int TutorId { get; set; }
}