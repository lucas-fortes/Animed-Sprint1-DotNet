using AnimedApi.Data;
using AnimedApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimedApi.Repositories;

public class TutorRepository
{
    private readonly AnimedDbContext _context;

    public TutorRepository(AnimedDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tutor>> ListarAsync()
    {
        return await _context.Tutores
            .OrderBy(tutor => tutor.Nome)
            .ToListAsync();
    }

    public async Task<Tutor?> BuscarPorIdAsync(int id)
    {
        return await _context.Tutores
            .FirstOrDefaultAsync(tutor => tutor.Id == id);
    }

    public async Task<Tutor?> BuscarPorCpfAsync(string cpf)
    {
        return await _context.Tutores
            .FirstOrDefaultAsync(tutor => tutor.Cpf == cpf);
    }

    public async Task<Tutor> CriarAsync(Tutor tutor)
    {
        _context.Tutores.Add(tutor);
        await _context.SaveChangesAsync();

        return tutor;
    }

    public async Task AtualizarAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RemoverAsync(int id)
    {
        Tutor? tutor = await BuscarPorIdAsync(id);

        if (tutor is null)
        {
            return false;
        }

        _context.Tutores.Remove(tutor);
        await _context.SaveChangesAsync();

        return true;
    }
}