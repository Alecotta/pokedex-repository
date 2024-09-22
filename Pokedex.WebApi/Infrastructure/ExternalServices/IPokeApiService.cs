using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.PokeApi;

namespace Pokedex.WebApi.Infrastructure.ExternalServices
{
    public interface IPokeApiService
    {
        Task<Result<PokemonSpecieModel?>> GetPokemonSpecieModelByNameAsync(string pokemonName);
    }
}