using AnimedApi.Dtos;
using AnimedApi.Models;
using AnimedApi.Repositories;

namespace AnimedApi.Services;

public class PetService
{
    private readonly PetRepository _petRepository;

    public PetService(PetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public async Task<List<PetResponse>> ListarAsync()
    {
        List<Pet> pets = await _petRepository.ListarAsync();

        return pets.Select(MapearParaResponse).ToList();
    }

    public async Task<List<PetResponse>> ListarPorTutorAsync(int tutorId)
    {
        List<Pet> pets = await _petRepository.ListarPorTutorAsync(tutorId);

        return pets.Select(MapearParaResponse).ToList();
    }

    public async Task<PetResponse?> BuscarPorIdAsync(int id)
    {
        Pet? pet = await _petRepository.BuscarPorIdAsync(id);

        if (pet is null)
        {
            return null;
        }

        return MapearParaResponse(pet);
    }

    public async Task<PetResponse> CriarAsync(PetRequest request)
    {
        ValidarRequest(request);

        Tutor? tutor = await _petRepository.BuscarTutorPorIdAsync(request.TutorId);

        if (tutor is null)
        {
            throw new ArgumentException("Tutor informado não foi encontrado.");
        }

        Pet pet = new Pet
        {
            Nome = request.Nome.Trim(),
            Especie = request.Especie.Trim(),
            Raca = request.Raca.Trim(),
            Idade = request.Idade,
            Peso = request.Peso,
            TutorId = request.TutorId,
            CriadoEm = DateTime.Now
        };

        Pet petCriado = await _petRepository.CriarAsync(pet);

        Pet? petCompleto = await _petRepository.BuscarPorIdAsync(petCriado.Id);

        return MapearParaResponse(petCompleto ?? petCriado);
    }

    public async Task<PetResponse?> AtualizarAsync(int id, PetRequest request)
    {
        ValidarRequest(request);

        Pet? pet = await _petRepository.BuscarPorIdAsync(id);

        if (pet is null)
        {
            return null;
        }

        Tutor? tutor = await _petRepository.BuscarTutorPorIdAsync(request.TutorId);

        if (tutor is null)
        {
            throw new ArgumentException("Tutor informado não foi encontrado.");
        }

        pet.Nome = request.Nome.Trim();
        pet.Especie = request.Especie.Trim();
        pet.Raca = request.Raca.Trim();
        pet.Idade = request.Idade;
        pet.Peso = request.Peso;
        pet.TutorId = request.TutorId;

        await _petRepository.AtualizarAsync();

        Pet? petAtualizado = await _petRepository.BuscarPorIdAsync(id);

        return MapearParaResponse(petAtualizado ?? pet);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _petRepository.RemoverAsync(id);
    }

    private static PetResponse MapearParaResponse(Pet pet)
    {
        return new PetResponse
        {
            Id = pet.Id,
            Nome = pet.Nome,
            Especie = pet.Especie,
            Raca = pet.Raca,
            Idade = pet.Idade,
            Peso = pet.Peso,
            TutorId = pet.TutorId,
            NomeTutor = pet.Tutor?.Nome ?? string.Empty,
            CriadoEm = pet.CriadoEm
        };
    }

    private static void ValidarRequest(PetRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            throw new ArgumentException("O nome do pet é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Especie))
        {
            throw new ArgumentException("A espécie do pet é obrigatória.");
        }

        if (request.Idade < 0)
        {
            throw new ArgumentException("A idade do pet não pode ser negativa.");
        }

        if (request.Peso <= 0)
        {
            throw new ArgumentException("O peso do pet deve ser maior que zero.");
        }

        if (request.TutorId <= 0)
        {
            throw new ArgumentException("O tutor do pet é obrigatório.");
        }
    }
}