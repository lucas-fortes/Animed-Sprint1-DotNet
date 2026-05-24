using AnimedApi.Data;
using AnimedApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimedApi.Repositories;

public class ConsultaRepository
{
    private readonly AnimedDbContext _context;

    public ConsultaRepository(AnimedDbContext context)
    {
        _context = context;
    }

    public async Task<List<Consulta>> ListarAsync()
    {
        return await _context.Consultas
            .Include(consulta => consulta.Tutor)
            .Include(consulta => consulta.Pet)
            .OrderByDescending(consulta => consulta.DataConsulta)
            .ToListAsync();
    }

    public async Task<Consulta?> BuscarPorIdAsync(int id)
    {
        return await _context.Consultas
            .Include(consulta => consulta.Tutor)
            .Include(consulta => consulta.Pet)
            .FirstOrDefaultAsync(consulta => consulta.Id == id);
    }

    public async Task<Tutor?> BuscarTutorPorIdAsync(int tutorId)
    {
        return await _context.Tutores
            .FirstOrDefaultAsync(tutor => tutor.Id == tutorId);
    }

    public async Task<Pet?> BuscarPetPorIdAsync(int petId)
    {
        return await _context.Pets
            .FirstOrDefaultAsync(pet => pet.Id == petId);
    }

    public async Task<Consulta> CriarAsync(Consulta consulta)
    {
        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();

        return consulta;
    }

    public async Task AtualizarAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> RemoverAsync(int id)
    {
        Consulta? consulta = await BuscarPorIdAsync(id);

        if (consulta is null)
        {
            return false;
        }

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();

        return true;
    }
}