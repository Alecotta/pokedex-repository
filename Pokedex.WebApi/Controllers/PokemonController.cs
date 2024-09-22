using Microsoft.AspNetCore.Mvc;
using Pokedex.WebApi.DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace Pokedex.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PokemonController : ControllerBase
    {

        private readonly ILogger<PokemonController> _logger;

        public PokemonController(ILogger<PokemonController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> GetPokemonByName
        (
            [FromRoute][Required] string pokemonName
        )
        {
            var response = new PokemonResponseDTO<string>
            {
                Success = true
            };

            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                response.Success = false;
                response.ErrorMessage = "Pokemon name must be defined.";
                return BadRequest(response);
            }
            return Ok();
        }
    }
}