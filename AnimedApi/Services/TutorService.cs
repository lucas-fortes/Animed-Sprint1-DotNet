using System.Text.RegularExpressions;
using AnimedApi.Dtos;
using AnimedApi.Models;
using AnimedApi.Repositories;

namespace AnimedApi.Services;

public class TutorService
{
    private readonly TutorRepository _tutorRepository;

    public TutorService(TutorRepository tutorRepository)
    {
        _tutorRepository = tutorRepository;
    }

    public async Task<List<TutorResponse>> ListarAsync()
    {
        List<Tutor> tutores = await _tutorRepository.ListarAsync();

        return tutores.Select(MapearParaResponse).ToList();
    }

    public async Task<TutorResponse?> BuscarPorIdAsync(int id)
    {
        Tutor? tutor = await _tutorRepository.BuscarPorIdAsync(id);

        if (tutor is null)
        {
            return null;
        }

        return MapearParaResponse(tutor);
    }

    public async Task<TutorResponse> CriarAsync(TutorRequest request)
    {
        ValidarRequest(request);

        string cpfNormalizado = NormalizarCpf(request.Cpf);

        Tutor? tutorExistente = await _tutorRepository.BuscarPorCpfAsync(cpfNormalizado);

        if (tutorExistente is not null)
        {
            throw new ArgumentException("Já existe um tutor cadastrado com esse CPF.");
        }

        Tutor tutor = new Tutor
        {
            Nome = request.Nome.Trim(),
            Cpf = cpfNormalizado,
            Email = request.Email.Trim(),
            Telefone = request.Telefone.Trim(),
            CriadoEm = DateTime.Now
        };

        Tutor tutorCriado = await _tutorRepository.CriarAsync(tutor);

        return MapearParaResponse(tutorCriado);
    }

    public async Task<TutorResponse?> AtualizarAsync(int id, TutorRequest request)
    {
        ValidarRequest(request);

        Tutor? tutor = await _tutorRepository.BuscarPorIdAsync(id);

        if (tutor is null)
        {
            return null;
        }

        string cpfNormalizado = NormalizarCpf(request.Cpf);

        Tutor? tutorComMesmoCpf = await _tutorRepository.BuscarPorCpfAsync(cpfNormalizado);

        if (tutorComMesmoCpf is not null && tutorComMesmoCpf.Id != id)
        {
            throw new ArgumentException("Já existe outro tutor cadastrado com esse CPF.");
        }

        tutor.Nome = request.Nome.Trim();
        tutor.Cpf = cpfNormalizado;
        tutor.Email = request.Email.Trim();
        tutor.Telefone = request.Telefone.Trim();

        await _tutorRepository.AtualizarAsync();

        return MapearParaResponse(tutor);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _tutorRepository.RemoverAsync(id);
    }

    private static TutorResponse MapearParaResponse(Tutor tutor)
    {
        return new TutorResponse
        {
            Id = tutor.Id,
            Nome = tutor.Nome,
            Cpf = tutor.Cpf,
            Email = tutor.Email,
            Telefone = tutor.Telefone,
            CriadoEm = tutor.CriadoEm
        };
    }

    private static string NormalizarCpf(string cpf)
    {
        return Regex.Replace(cpf, @"\D", "");
    }

    private static void ValidarRequest(TutorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            throw new ArgumentException("O nome do tutor é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Cpf))
        {
            throw new ArgumentException("O CPF do tutor é obrigatório.");
        }

        if (NormalizarCpf(request.Cpf).Length != 11)
        {
            throw new ArgumentException("O CPF do tutor deve conter 11 números.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("O e-mail do tutor é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Telefone))
        {
            throw new ArgumentException("O telefone do tutor é obrigatório.");
        }
    }
}