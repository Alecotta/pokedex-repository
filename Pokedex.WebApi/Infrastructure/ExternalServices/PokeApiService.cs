using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.PokeApi;

namespace Pokedex.WebApi.Infrastructure.ExternalServices
{
    public class PokeApiService : IPokeApiService
    {
        private readonly HttpClient _httpClient;

        public PokeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<PokemonSpecieModel?>> GetPokemonSpecieModelByNameAsync(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            var endpoint = $"pokemon-species/{pokemonName}";

            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var pokemonSpecieModel = await response.Content.ReadFromJsonAsync<PokemonSpecieModel?>();

                return Result<PokemonSpecieModel?>.Success(pokemonSpecieModel);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Result<PokemonSpecieModel?>.Failure("Resource not found.");
            }
            catch
            {
                return Result<PokemonSpecieModel?>.Failure("External Service Error.");
            }
        }
    }
}
