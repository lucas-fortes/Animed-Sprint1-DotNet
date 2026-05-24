using AnimedApi.Dtos;
using AnimedApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnimedApi.Controllers;

[ApiController]
[Route("api/tutores")]
public class TutoresController : ControllerBase
{
    private readonly TutorService _tutorService;

    public TutoresController(TutorService tutorService)
    {
        _tutorService = tutorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TutorResponse>>> Listar()
    {
        List<TutorResponse> tutores = await _tutorService.ListarAsync();

        return Ok(tutores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TutorResponse>> BuscarPorId(int id)
    {
        TutorResponse? tutor = await _tutorService.BuscarPorIdAsync(id);

        if (tutor is null)
        {
            return NotFound(new { mensagem = "Tutor não encontrado." });
        }

        return Ok(tutor);
    }

    [HttpPost]
    public async Task<ActionResult<TutorResponse>> Criar([FromBody] TutorRequest request)
    {
        try
        {
            TutorResponse tutor = await _tutorService.CriarAsync(request);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = tutor.Id },
                tutor
            );
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TutorResponse>> Atualizar(
        int id,
        [FromBody] TutorRequest request
    )
    {
        try
        {
            TutorResponse? tutor = await _tutorService.AtualizarAsync(id, request);

            if (tutor is null)
            {
                return NotFound(new { mensagem = "Tutor não encontrado." });
            }

            return Ok(tutor);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        bool removido = await _tutorService.RemoverAsync(id);

        if (removido == false)
        {
            return NotFound(new { mensagem = "Tutor não encontrado." });
        }

        return NoContent();
    }
}