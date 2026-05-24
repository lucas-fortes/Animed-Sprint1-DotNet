using AnimedApi.Dtos;
using AnimedApi.Models;
using AnimedApi.Repositories;

namespace AnimedApi.Services;

public class ConsultaService
{
    private readonly ConsultaRepository _consultaRepository;

    public ConsultaService(ConsultaRepository consultaRepository)
    {
        _consultaRepository = consultaRepository;
    }

    public async Task<List<ConsultaResponse>> ListarAsync(
        int? tutorId,
        int? petId,
        string? data
    )
    {
        List<Consulta> consultas = await _consultaRepository.ListarAsync();

        if (tutorId.HasValue)
        {
            consultas = consultas
                .Where(consulta => consulta.TutorId == tutorId.Value)
                .ToList();
        }

        if (petId.HasValue)
        {
            consultas = consultas
                .Where(consulta => consulta.PetId == petId.Value)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(data))
        {
            bool dataValida = DateTime.TryParse(data, out DateTime dataConvertida);

            if (dataValida == false)
            {
                throw new ArgumentException("A data informada é inválida.");
            }

            consultas = consultas
                .Where(consulta => consulta.DataConsulta.Date == dataConvertida.Date)
                .ToList();
        }

        return consultas.Select(MapearParaResponse).ToList();
    }

    public async Task<ConsultaResponse?> BuscarPorIdAsync(int id)
    {
        Consulta? consulta = await _consultaRepository.BuscarPorIdAsync(id);

        if (consulta is null)
        {
            return null;
        }

        return MapearParaResponse(consulta);
    }

    public async Task<ConsultaResponse> CriarAsync(ConsultaRequest request)
    {
        ValidarRequest(request);

        Tutor? tutor = await _consultaRepository.BuscarTutorPorIdAsync(request.TutorId);

        if (tutor is null)
        {
            throw new ArgumentException("Tutor informado não foi encontrado.");
        }

        Pet? pet = await _consultaRepository.BuscarPetPorIdAsync(request.PetId);

        if (pet is null)
        {
            throw new ArgumentException("Pet informado não foi encontrado.");
        }

        if (pet.TutorId != request.TutorId)
        {
            throw new ArgumentException("O pet informado não pertence ao tutor informado.");
        }

        Consulta consulta = new Consulta
        {
            DataConsulta = request.DataConsulta,
            Motivo = request.Motivo.Trim(),
            Diagnostico = request.Diagnostico.Trim(),
            Tratamento = request.Tratamento.Trim(),
            Observacoes = request.Observacoes.Trim(),
            NivelUrgencia = request.NivelUrgencia.Trim(),
            TutorId = request.TutorId,
            PetId = request.PetId,
            CriadoEm = DateTime.Now
        };

        Consulta consultaCriada = await _consultaRepository.CriarAsync(consulta);

        Consulta? consultaCompleta = await _consultaRepository.BuscarPorIdAsync(consultaCriada.Id);

        return MapearParaResponse(consultaCompleta ?? consultaCriada);
    }

    public async Task<ConsultaResponse?> AtualizarAsync(int id, ConsultaRequest request)
    {
        ValidarRequest(request);

        Consulta? consulta = await _consultaRepository.BuscarPorIdAsync(id);

        if (consulta is null)
        {
            return null;
        }

        Tutor? tutor = await _consultaRepository.BuscarTutorPorIdAsync(request.TutorId);

        if (tutor is null)
        {
            throw new ArgumentException("Tutor informado não foi encontrado.");
        }

        Pet? pet = await _consultaRepository.BuscarPetPorIdAsync(request.PetId);

        if (pet is null)
        {
            throw new ArgumentException("Pet informado não foi encontrado.");
        }

        if (pet.TutorId != request.TutorId)
        {
            throw new ArgumentException("O pet informado não pertence ao tutor informado.");
        }

        consulta.DataConsulta = request.DataConsulta;
        consulta.Motivo = request.Motivo.Trim();
        consulta.Diagnostico = request.Diagnostico.Trim();
        consulta.Tratamento = request.Tratamento.Trim();
        consulta.Observacoes = request.Observacoes.Trim();
        consulta.NivelUrgencia = request.NivelUrgencia.Trim();
        consulta.TutorId = request.TutorId;
        consulta.PetId = request.PetId;

        await _consultaRepository.AtualizarAsync();

        Consulta? consultaAtualizada = await _consultaRepository.BuscarPorIdAsync(id);

        return MapearParaResponse(consultaAtualizada ?? consulta);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _consultaRepository.RemoverAsync(id);
    }

    private static ConsultaResponse MapearParaResponse(Consulta consulta)
    {
        return new ConsultaResponse
        {
            Id = consulta.Id,
            DataConsulta = consulta.DataConsulta,
            Motivo = consulta.Motivo,
            Diagnostico = consulta.Diagnostico,
            Tratamento = consulta.Tratamento,
            Observacoes = consulta.Observacoes,
            NivelUrgencia = consulta.NivelUrgencia,
            TutorId = consulta.TutorId,
            NomeTutor = consulta.Tutor?.Nome ?? string.Empty,
            PetId = consulta.PetId,
            NomePet = consulta.Pet?.Nome ?? string.Empty,
            CriadoEm = consulta.CriadoEm
        };
    }

    private static void ValidarRequest(ConsultaRequest request)
    {
        if (request.TutorId <= 0)
        {
            throw new ArgumentException("O tutor da consulta é obrigatório.");
        }

        if (request.PetId <= 0)
        {
            throw new ArgumentException("O pet da consulta é obrigatório.");
        }

        if (request.DataConsulta == default)
        {
            throw new ArgumentException("A data da consulta é obrigatória.");
        }

        if (string.IsNullOrWhiteSpace(request.Motivo))
        {
            throw new ArgumentException("O motivo da consulta é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.NivelUrgencia))
        {
            throw new ArgumentException("O nível de urgência é obrigatório.");
        }
    }
}