using Moq;
using Pokedex.Test.Helpers;
using Pokedex.WebApi.Infrastructure.ExternalServices;
using Pokedex.WebApi.Models.PokeApi;
using System.Net;

namespace Pokedex.Test.Infrastructure
{
    public class PokeApiServiceTest
    {
        private readonly Uri _baseAddress = new("https://pokeapi.co/api/v2");

        [Fact]
        public async Task GetPokemonSpecieModelByNameAsync_ThrowsArgumentNullExceptionWhenPokemonNameIsNull()
        {
            //Arrange
            string? pokemonName = null;
            var mockHttpClient = new Mock<HttpClient>();
            var pokeApiService = new PokeApiService(mockHttpClient.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokeApiService.GetPokemonSpecieModelByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonSpecieModelByNameAsync_ThrowsArgumentNullExceptionWhenPokemonNameIsEmpty()
        {
            //Arrange
            string? pokemonName = string.Empty;
            var mockHttpClient = new Mock<HttpClient>();
            var pokeApiService = new PokeApiService(mockHttpClient.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokeApiService.GetPokemonSpecieModelByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonSpecieModelByNameAsync_ReturnNullWhenPokemonNotFoundAndStatusCode404()
        {
            //Arrange
            var pokemonName = "pokemon_test";
            var mockHttpMessageHandler = MockHttpMessageHandler.ReturnMockHttpResponse<object?>(HttpStatusCode.NotFound, null);
            
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = _baseAddress
            };
            
            var pokeApiService = new PokeApiService(mockHttpClient);

            //Act
            var result = await pokeApiService.GetPokemonSpecieModelByNameAsync(pokemonName);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(result.StatusCode, HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetPokemonSpecieModelByNameAsync_HandleErrorWhenExternalServiceIsNotWorking()
        {
            //Arrange
            string? pokemonName = "pokemon_test";
            var mockHttpMessageHandler = MockHttpMessageHandler.ReturnMockHttpResponse<object?>(HttpStatusCode.BadGateway, null);

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = _baseAddress
            };

            var pokeApiService = new PokeApiService(mockHttpClient);

            //Act
            var result = await pokeApiService.GetPokemonSpecieModelByNameAsync(pokemonName);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(result.StatusCode, HttpStatusCode.BadGateway);
        }

        [Fact]
        public async Task GetPokemonSpecieModelByNameAsync_ReturnSuccessAndResultNotNullWhenPokemonFoundAndStatusCode200()
        {
            //Arrange
            string? pokemonName = "pokemon_test";
            var mockHttpResponse = new PokemonSpecieModel
            {
                Name = "mock_pokemon",
                Id = It.IsAny<int>()
            };
            var mockHttpMessageHandler = MockHttpMessageHandler.ReturnMockHttpResponse(HttpStatusCode.OK, mockHttpResponse); ;

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = _baseAddress
            };

            var pokeApiService = new PokeApiService(mockHttpClient);

            //Act
            var result = await pokeApiService.GetPokemonSpecieModelByNameAsync(pokemonName);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
        }
    }
}
