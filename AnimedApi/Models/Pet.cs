namespace AnimedApi.Models;

public class Pet
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string Raca { get; set; } = string.Empty;

    public int Idade { get; set; }

    public decimal Peso { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public int TutorId { get; set; }

    public Tutor? Tutor { get; set; }

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

    public ICollection<Vacina> Vacinas { get; set; } = new List<Vacina>();
}