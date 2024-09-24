using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Models;

namespace Pokedex.WebApi.Services
{
    public interface IPokemonService
    {
        Task<ResultModel<PokemonResponseDTO?>> GetPokemonByNameAsync(string pokemonName, bool translatePokemonDescription = false);
    }
}