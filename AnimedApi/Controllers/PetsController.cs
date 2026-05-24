using AnimedApi.Dtos;
using AnimedApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnimedApi.Controllers;

[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    private readonly PetService _petService;

    public PetsController(PetService petService)
    {
        _petService = petService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PetResponse>>> Listar()
    {
        List<PetResponse> pets = await _petService.ListarAsync();

        return Ok(pets);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PetResponse>> BuscarPorId(int id)
    {
        PetResponse? pet = await _petService.BuscarPorIdAsync(id);

        if (pet is null)
        {
            return NotFound(new { mensagem = "Pet não encontrado." });
        }

        return Ok(pet);
    }

    [HttpGet("tutor/{tutorId:int}")]
    public async Task<ActionResult<List<PetResponse>>> ListarPorTutor(int tutorId)
    {
        List<PetResponse> pets = await _petService.ListarPorTutorAsync(tutorId);

        return Ok(pets);
    }

    [HttpPost]
    public async Task<ActionResult<PetResponse>> Criar([FromBody] PetRequest request)
    {
        try
        {
            PetResponse pet = await _petService.CriarAsync(request);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = pet.Id },
                pet
            );
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PetResponse>> Atualizar(
        int id,
        [FromBody] PetRequest request
    )
    {
        try
        {
            PetResponse? pet = await _petService.AtualizarAsync(id, request);

            if (pet is null)
            {
                return NotFound(new { mensagem = "Pet não encontrado." });
            }

            return Ok(pet);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensagem = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        bool removido = await _petService.RemoverAsync(id);

        if (removido == false)
        {
            return NotFound(new { mensagem = "Pet não encontrado." });
        }

        return NoContent();
    }
}