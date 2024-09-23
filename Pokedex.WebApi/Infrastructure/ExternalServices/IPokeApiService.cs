using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.PokeApi;

namespace Pokedex.WebApi.Infrastructure.ExternalServices
{
    public interface IPokeApiService
    {
        Task<ResultModel<PokemonSpecieModel?>> GetPokemonSpecieModelByNameAsync(string pokemonName);
    }
}