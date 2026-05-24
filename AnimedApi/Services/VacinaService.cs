using AnimedApi.Dtos;
using AnimedApi.Models;
using AnimedApi.Repositories;

namespace AnimedApi.Services;

public class VacinaService
{
    private readonly VacinaRepository _vacinaRepository;

    public VacinaService(VacinaRepository vacinaRepository)
    {
        _vacinaRepository = vacinaRepository;
    }

    public async Task<List<VacinaResponse>> ListarAsync(int? petId)
    {
        List<Vacina> vacinas = petId.HasValue
            ? await _vacinaRepository.ListarPorPetAsync(petId.Value)
            : await _vacinaRepository.ListarAsync();

        return vacinas.Select(MapearParaResponse).ToList();
    }

    public async Task<VacinaResponse?> BuscarPorIdAsync(int id)
    {
        Vacina? vacina = await _vacinaRepository.BuscarPorIdAsync(id);

        if (vacina is null)
        {
            return null;
        }

        return MapearParaResponse(vacina);
    }

    public async Task<VacinaResponse> CriarAsync(VacinaRequest request)
    {
        ValidarRequest(request);

        Pet? pet = await _vacinaRepository.BuscarPetPorIdAsync(request.PetId);

        if (pet is null)
        {
            throw new ArgumentException("Pet informado não foi encontrado.");
        }

        Vacina vacina = new Vacina
        {
            Nome = request.Nome.Trim(),
            DataAplicacao = request.DataAplicacao,
            DataProximaDose = request.DataProximaDose,
            Observacoes = request.Observacoes.Trim(),
            PetId = request.PetId,
            CriadoEm = DateTime.Now
        };

        Vacina vacinaCriada = await _vacinaRepository.CriarAsync(vacina);

        Vacina? vacinaCompleta = await _vacinaRepository.BuscarPorIdAsync(vacinaCriada.Id);

        return MapearParaResponse(vacinaCompleta ?? vacinaCriada);
    }

    public async Task<VacinaResponse?> AtualizarAsync(int id, VacinaRequest request)
    {
        ValidarRequest(request);

        Vacina? vacina = await _vacinaRepository.BuscarPorIdAsync(id);

        if (vacina is null)
        {
            return null;
        }

        Pet? pet = await _vacinaRepository.BuscarPetPorIdAsync(request.PetId);

        if (pet is null)
        {
            throw new ArgumentException("Pet informado não foi encontrado.");
        }

        vacina.Nome = request.Nome.Trim();
        vacina.DataAplicacao = request.DataAplicacao;
        vacina.DataProximaDose = request.DataProximaDose;
        vacina.Observacoes = request.Observacoes.Trim();
        vacina.PetId = request.PetId;

        await _vacinaRepository.AtualizarAsync();

        Vacina? vacinaAtualizada = await _vacinaRepository.BuscarPorIdAsync(id);

        return MapearParaResponse(vacinaAtualizada ?? vacina);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _vacinaRepository.RemoverAsync(id);
    }

    private static VacinaResponse MapearParaResponse(Vacina vacina)
    {
        return new VacinaResponse
        {
            Id = vacina.Id,
            Nome = vacina.Nome,
            DataAplicacao = vacina.DataAplicacao,
            DataProximaDose = vacina.DataProximaDose,
            Observacoes = vacina.Observacoes,
            PetId = vacina.PetId,
            NomePet = vacina.Pet?.Nome ?? string.Empty,
            CriadoEm = vacina.CriadoEm
        };
    }

    private static void ValidarRequest(VacinaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            throw new ArgumentException("O nome da vacina é obrigatório.");
        }

        if (request.DataAplicacao == default)
        {
            throw new ArgumentException("A data de aplicação da vacina é obrigatória.");
        }

        if (request.PetId <= 0)
        {
            throw new ArgumentException("O pet da vacina é obrigatório.");
        }
    }
}