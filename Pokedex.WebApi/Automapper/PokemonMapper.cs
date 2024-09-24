using AutoMapper;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Models.PokeApi;
using System.Text.RegularExpressions;

namespace Pokedex.WebApi.Automapper
{
    public class PokemonMapper : Profile
    {
        private const string CONTROL_CHARACTER_PATTERN = @"\p{C}+";

        public PokemonMapper()
        {
            CreateMap<PokemonSpecieModel, PokemonResponseDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FlavorTextEntries == null || !src.FlavorTextEntries.Any() ? null : Regex.Replace(src.FlavorTextEntries.First().FlavorText, CONTROL_CHARACTER_PATTERN, " ")))
                .ForMember(dest => dest.Habitat, opt => opt.MapFrom(src => src.Habitat == null ? null : Regex.Replace(src.Habitat.Name, CONTROL_CHARACTER_PATTERN, " ")));
        }
    }
}
