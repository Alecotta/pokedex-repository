using AutoMapper;
using Moq;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Infrastructure.ExternalServices;
using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.PokeApi;
using Pokedex.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Test.Services
{
    public class PokemonServiceTest
    {
        [Fact]
        public async Task GetPokemonByNameAsync_ThrowArgumentNullExceptionWhenPokemonNameIsNull()
        {
            //Arrange
            string? pokemonName = null;
            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockMapper.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokemonService.GetPokemonByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ThrowArgumentNullExceptionWhenPokemonNameIsEmpty()
        {
            //Arrange
            string pokemonName = string.Empty;
            var mockPokeApiService = new Mock<IPokeApiService>();
            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockMapper.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokemonService.GetPokemonByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ReturnSuccessTrueAndResultNotNullWhenPokemonFound()
        {
            //Arrange
            string pokemonName = "pokemon_test";
            var pokemonSpecieModelMock = new PokemonSpecieModel
            {
                Name = pokemonName,
                IsLegendary = true,
                FlavorTextEntries = new List<FlavorTextEntry>
                    {
                        new FlavorTextEntry
                        {
                            FlavorText = "flavor_text_test"
                        }
                    },
                Habitat = new Habitat
                {
                    Name = "habitat_test"
                }
            };
            var pokemonResponseDTOMock = new PokemonResponseDTO
            {
                Description = "flavor_text_test",
                Habitat = "habitat_test",
                IsLegendary = true,
                Name = pokemonName
            };

            var mockPokeApiService = new Mock<IPokeApiService>();

            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(Result<PokemonSpecieModel?>.Success(pokemonSpecieModelMock));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<PokemonResponseDTO>(It.IsAny<PokemonSpecieModel>())).Returns(pokemonResponseDTOMock);
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName);

            //Act & Assert
            Assert.True(getPokemonByNameAsyncResult.IsSuccess);
            Assert.NotNull(getPokemonByNameAsyncResult.Data);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Name, pokemonName);
            Assert.Equal(getPokemonByNameAsyncResult.Data.IsLegendary, pokemonResponseDTOMock.IsLegendary);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Habitat, pokemonResponseDTOMock.Habitat);
            Assert.Equal(getPokemonByNameAsyncResult.Data.Description, pokemonResponseDTOMock.Description);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ReturnSuccessFalseAndResultNullWhenPokemonNotFound()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(Result<PokemonSpecieModel?>.Failure("Pokemon Not Found."));

            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName);

            //Act & Assert
            Assert.False(getPokemonByNameAsyncResult.IsSuccess);
            Assert.Null(getPokemonByNameAsyncResult.Data);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ReturnSuccessFalseAndResultNullWhenExternalServiceNotWorking()
        {
            //Arrange
            string pokemonName = "pokemon_test";

            var mockPokeApiService = new Mock<IPokeApiService>();
            mockPokeApiService.Setup(m => m.GetPokemonSpecieModelByNameAsync(pokemonName))
                .ReturnsAsync(Result<PokemonSpecieModel?>.Failure("Internal Server Error."));

            var mockMapper = new Mock<IMapper>();
            var pokemonService = new PokemonService(mockPokeApiService.Object, mockMapper.Object);

            //Act
            var getPokemonByNameAsyncResult = await pokemonService.GetPokemonByNameAsync(pokemonName);

            //Act & Assert
            Assert.False(getPokemonByNameAsyncResult.IsSuccess);
            Assert.Null(getPokemonByNameAsyncResult.Data);
        }
    }
}
