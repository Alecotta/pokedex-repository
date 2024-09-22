using AutoMapper;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.PokeApi;

namespace Pokedex.WebApi.Automapper
{
    public class PokemonMapper : Profile
    {
        public PokemonMapper()
        {
            CreateMap<PokemonSpecieModel, PokemonResponseDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FlavorTextEntries == null || !src.FlavorTextEntries.Any() ? null : src.FlavorTextEntries.First().FlavorText))
                .ForMember(dest => dest.Habitat, opt => opt.MapFrom(src => src.Habitat == null ? null : src.Habitat.Name));
        }
    }
}
