using AnimedApi.Dtos;
using AnimedApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnimedApi.Controllers;

[ApiController]
[Route("api/vacinas")]
public class VacinasController : ControllerBase
{
    private readonly VacinaService _vacinaService;

    public VacinasController(VacinaService vacinaService)
    {
        _vacinaService = vacinaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<VacinaResponse>>> Listar(
        [FromQuery] int? petId
    )
    {
        List<VacinaResponse> vacinas = await _vacinaService.ListarAsync(petId);

        return Ok(vacinas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VacinaResponse>> BuscarPorId(int id)
    {
        VacinaResponse? vacina = await _vacinaService.BuscarPorIdAsync(id);

        if (vacina is null)
        {
            return NotFound(new { mensagem = "Vacina não encontrada." });
        }

        return Ok(vacina);
    }

    [HttpPost]
    public async Task<ActionResult<VacinaResponse>> Criar(
        [FromBody] VacinaRequest request
    )
    {
        try
        {
            VacinaResponse vacina = await _vacinaService.CriarAsync(request);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = vacina.Id },
                vacina
            );
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VacinaResponse>> Atualizar(
        int id,
        [FromBody] VacinaRequest request
    )
    {
        try
        {
            VacinaResponse? vacina = await _vacinaService.AtualizarAsync(
                id,
                request
            );

            if (vacina is null)
            {
                return NotFound(new { mensagem = "Vacina não encontrada." });
            }

            return Ok(vacina);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        bool removido = await _vacinaService.RemoverAsync(id);

        if (removido == false)
        {
            return NotFound(new { mensagem = "Vacina não encontrada." });
        }

        return NoContent();
    }
}