using Microsoft.AspNetCore.Mvc;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Helpers;
using Pokedex.WebApi.Models;
using Pokedex.WebApi.Services;
using System.ComponentModel.DataAnnotations;

namespace Pokedex.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController
        (
            IPokemonService pokemonService,
            ILogger<PokemonController> logger
        )
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> GetPokemonByName
        (
            [FromRoute][Required] string? pokemonName
        )
        {
            ResultModel<PokemonResponseDTO> response;

            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                response = new(false, null, "Pokemon name must be defined.");
                return BadRequest(response);
            }

            var pokemonResponseDTO = await _pokemonService.GetPokemonByNameAsync(pokemonName);
            if 
            (
                !pokemonResponseDTO.IsSuccess
            )
            {
                if (pokemonResponseDTO.StatusCode.HasValue)
                {
                    return HttpStatusCodeHelper.ToObjectResult(pokemonResponseDTO.StatusCode.Value, pokemonResponseDTO);
                }
                else
                {
                    return Problem(pokemonResponseDTO.ErrorMessage);
                }
            }

            return Ok(pokemonResponseDTO);
        }
    }
}