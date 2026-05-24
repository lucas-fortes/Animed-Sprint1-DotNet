namespace AnimedApi.Dtos;

public class PetResponse
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string Raca { get; set; } = string.Empty;

    public int Idade { get; set; }

    public decimal Peso { get; set; }

    public int TutorId { get; set; }

    public string NomeTutor { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}