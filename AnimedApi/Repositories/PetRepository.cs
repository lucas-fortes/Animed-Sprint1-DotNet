using AnimedApi.Data;
using AnimedApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimedApi.Repositories;

public class PetRepository
{
    private readonly AnimedDbContext _context;

    public PetRepository(AnimedDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pet>> ListarAsync()
    {
        return await _context.Pets
            .Include(pet => pet.Tutor)
            .OrderBy(pet => pet.Nome)
            .ToListAsync();
    }

    public async Task<List<Pet>> ListarPorTutorAsync(int tutorId)
    {
        return await _context.Pets
            .Include(pet => pet.Tutor)
            .Where(pet => pet.TutorId == tutorId)
            .OrderBy(pet => pet.Nome)
            .ToListAsync();
    }

    public async Task<Pet?> BuscarPorIdAsync(int id)
    {
        return await _context.Pets
            .Include(pet => pet.Tutor)
            .FirstOrDefaultAsync(pet => pet.Id == id);
    }

    public async Task<Tutor?> BuscarTutorPorIdAsync(int tutorId)
    {
        return await _context.Tutores
            .FirstOrDefaultAsync(tutor => tutor.Id == tutorId);
    }

    public async Task<Pet> CriarAsync(Pet pet)
    {
        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();

        return pet;
    }

    public async Task AtualizarAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RemoverAsync(int id)
    {
        Pet? pet = await BuscarPorIdAsync(id);

        if (pet is null)
        {
            return false;
        }

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();

        return true;
    }
}