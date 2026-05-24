using AnimedApi.Dtos;
using AnimedApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnimedApi.Controllers;

[ApiController]
[Route("api/consultas")]
public class ConsultasController : ControllerBase
{
    private readonly ConsultaService _consultaService;

    public ConsultasController(ConsultaService consultaService)
    {
        _consultaService = consultaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ConsultaResponse>>> Listar(
        [FromQuery] int? tutorId,
        [FromQuery] int? petId,
        [FromQuery] string? data
    )
    {
        List<ConsultaResponse> consultas = await _consultaService.ListarAsync(
            tutorId,
            petId,
            data
        );

        return Ok(consultas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConsultaResponse>> BuscarPorId(int id)
    {
        ConsultaResponse? consulta = await _consultaService.BuscarPorIdAsync(id);

        if (consulta is null)
        {
            return NotFound(new { mensagem = "Consulta não encontrada." });
        }

        return Ok(consulta);
    }

    [HttpPost]
    public async Task<ActionResult<ConsultaResponse>> Criar(
        [FromBody] ConsultaRequest request
    )
    {
        try
        {
            ConsultaResponse consulta = await _consultaService.CriarAsync(request);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = consulta.Id },
                consulta
            );
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ConsultaResponse>> Atualizar(
        int id,
        [FromBody] ConsultaRequest request
    )
    {
        try
        {
            ConsultaResponse? consulta = await _consultaService.AtualizarAsync(
                id,
                request
            );

            if (consulta is null)
            {
                return NotFound(new { mensagem = "Consulta não encontrada." });
            }

            return Ok(consulta);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        bool removido = await _consultaService.RemoverAsync(id);

        if (removido == false)
        {
            return NotFound(new { mensagem = "Consulta não encontrada." });
        }

        return NoContent();
    }
}