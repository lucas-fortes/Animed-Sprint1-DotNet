namespace AnimedApi.Models;

public class Tutor
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}