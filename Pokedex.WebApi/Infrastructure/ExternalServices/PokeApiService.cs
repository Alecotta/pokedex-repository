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

        public async Task<ResultModel<PokemonSpecieModel?>> GetPokemonSpecieModelByNameAsync(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            var endpoint = $"pokemon-species/{pokemonName}";

            HttpResponseMessage? response;

            try
            {
                response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var pokemonSpecieModel = await response.Content.ReadFromJsonAsync<PokemonSpecieModel?>();

                return ResultModel<PokemonSpecieModel?>.Success(pokemonSpecieModel, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return ResultModel<PokemonSpecieModel?>.Failure(ex.Message, ex.StatusCode);
            }
            catch (Exception ex)
            {
                return ResultModel<PokemonSpecieModel?>.Failure(ex.Message);
            }
        }
    }
}
