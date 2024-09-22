using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Models;

namespace Pokedex.WebApi.Services
{
    public interface IPokemonService
    {
        Task<Result<PokemonResponseDTO?>> GetPokemonByNameAsync(string pokemonName);
    }
}