using AnimedApi.Data;
using AnimedApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimedApi.Repositories;

public class VacinaRepository
{
    private readonly AnimedDbContext _context;

    public VacinaRepository(AnimedDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vacina>> ListarAsync()
    {
        return await _context.Vacinas
            .Include(vacina => vacina.Pet)
            .OrderByDescending(vacina => vacina.DataAplicacao)
            .ToListAsync();
    }

    public async Task<List<Vacina>> ListarPorPetAsync(int petId)
    {
        return await _context.Vacinas
            .Include(vacina => vacina.Pet)
            .Where(vacina => vacina.PetId == petId)
            .OrderByDescending(vacina => vacina.DataAplicacao)
            .ToListAsync();
    }

    public async Task<Vacina?> BuscarPorIdAsync(int id)
    {
        return await _context.Vacinas
            .Include(vacina => vacina.Pet)
            .FirstOrDefaultAsync(vacina => vacina.Id == id);
    }

    public async Task<Pet?> BuscarPetPorIdAsync(int petId)
    {
        return await _context.Pets
            .FirstOrDefaultAsync(pet => pet.Id == petId);
    }

    public async Task<Vacina> CriarAsync(Vacina vacina)
    {
        _context.Vacinas.Add(vacina);
        await _context.SaveChangesAsync();

        return vacina;
    }

    public async Task AtualizarAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RemoverAsync(int id)
    {
        Vacina? vacina = await BuscarPorIdAsync(id);

        if (vacina is null)
        {
            return false;
        }

        _context.Vacinas.Remove(vacina);
        await _context.SaveChangesAsync();

        return true;
    }
}