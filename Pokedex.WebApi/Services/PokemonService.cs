using AutoMapper;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Infrastructure.ExternalServices;
using Pokedex.WebApi.Models;

namespace Pokedex.WebApi.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokeApiService _pokeApiService;
        private readonly IMapper _mapper;

        public PokemonService
        (
            IPokeApiService pokeApiService,
            IMapper mapper
        )
        {
            _pokeApiService = pokeApiService;
            _mapper = mapper;
        }

        public async Task<ResultModel<PokemonResponseDTO?>> GetPokemonByNameAsync(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            var pokemonSpecieModelResult = await _pokeApiService.GetPokemonSpecieModelByNameAsync(pokemonName);
            if (!pokemonSpecieModelResult.IsSuccess)
            {
                return ResultModel<PokemonResponseDTO?>.Failure(pokemonSpecieModelResult.ErrorMessage, pokemonSpecieModelResult.StatusCode);
            }


            var pokemonResponseDTO = _mapper.Map<PokemonResponseDTO>(pokemonSpecieModelResult.Data);

            return ResultModel<PokemonResponseDTO?>.Success(pokemonResponseDTO, pokemonSpecieModelResult.StatusCode); ;
        }
    }
}
