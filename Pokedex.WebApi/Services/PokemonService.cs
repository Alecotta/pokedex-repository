using AutoMapper;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Infrastructure.ExternalServices;
using Pokedex.WebApi.Models;

namespace Pokedex.WebApi.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokeApiService _pokeApiService;
        private readonly ITranslatorService _translatorService;
        private readonly IMapper _mapper;

        public PokemonService
        (
            IPokeApiService pokeApiService,
            ITranslatorService translatorService,
            IMapper mapper
        )
        {
            _pokeApiService = pokeApiService;
            _translatorService = translatorService;
            _mapper = mapper;
        }

        public async Task<ResultModel<PokemonResponseDTO?>> GetPokemonByNameAsync
        (
            string pokemonName,
            bool translatePokemonDescription = false
        )
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

            if (translatePokemonDescription && !string.IsNullOrWhiteSpace(pokemonResponseDTO.Description))
            {
                var translationType = pokemonResponseDTO.IsLegendary || pokemonResponseDTO.Habitat == "cave" ? TranslationType.SHAKESPEARE : TranslationType.YODA;
                var descriptionTranslation = await _translatorService.GetTraslation(pokemonResponseDTO.Description, translationType);
                if 
                (
                    descriptionTranslation.IsSuccess 
                    && !string.IsNullOrWhiteSpace(descriptionTranslation.Data?.Contents?.Translated)
                )
                {
                    pokemonResponseDTO.Description = descriptionTranslation.Data.Contents.Translated;
                }
            }

            return ResultModel<PokemonResponseDTO?>.Success(pokemonResponseDTO, pokemonSpecieModelResult.StatusCode); ;
        }
    }
}
