using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.WebApi.Controllers;
using Pokedex.WebApi.DTOs.Response;
using Pokedex.WebApi.Models;
using Pokedex.WebApi.Services;
using System.Net;

namespace Pokedex.Test.Controllers
{
    public class PokemonControllerTest
    {
        [Fact]
        public async Task GetPokemonByName_ReturnBadRequestWhenPokemonNameIsEmpty()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string pokemonName = string.Empty;
            var response = await pokemonController.GetPokemonByName(pokemonName) as BadRequestObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO>;

            //Assert
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
        }

        [Fact]
        public async Task GetPokemonByName_ReturnBadRequestWhenPokemonNameIsNull()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string? pokemonName = null;
            var response = await pokemonController.GetPokemonByName(pokemonName) as BadRequestObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO>;

            //Assert
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
        }

        [Fact]
        public async Task GetPokemonByName_ReturnNotFoundObjectResultWhenPokemonNotExists()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string? pokemonName = "pokemon_not_exists";
            mockPokemonService.Setup(m => m.GetPokemonByNameAsync(pokemonName, false))
                .ReturnsAsync(ResultModel<PokemonResponseDTO?>.Failure("Pokemon not found", HttpStatusCode.NotFound));
            
            var response = await pokemonController.GetPokemonByName(pokemonName) as ObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO?>;

            //Assert
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
            Assert.Null(responseValue.Data);
        }

        [Fact]
        public async Task GetPokemonByName_ReturnProblemObjectResultWhenHttpResponseError()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string? pokemonName = "pokemon_not_exists";
            mockPokemonService.Setup(m => m.GetPokemonByNameAsync(pokemonName, false))
                .ReturnsAsync(ResultModel<PokemonResponseDTO?>.Failure("Bad Gateway", HttpStatusCode.BadGateway));

            var response = await pokemonController.GetPokemonByName(pokemonName) as ObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO?>;

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.BadGateway);
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
            Assert.Null(responseValue.Data);
        }

        [Fact]
        public async Task GetPokemonByNameWithDescriptionTranslated_ReturnBadRequestWhenPokemonNameIsEmpty()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string pokemonName = string.Empty;
            var response = await pokemonController.GetPokemonByNameWithDescriptionTranslated(pokemonName) as BadRequestObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO>;

            //Assert
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
        }

        [Fact]
        public async Task GetPokemonByNameWithDescriptionTranslated_ReturnBadRequestWhenPokemonNameIsNull()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string? pokemonName = null;
            var response = await pokemonController.GetPokemonByNameWithDescriptionTranslated(pokemonName) as BadRequestObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO>;

            //Assert
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
        }

        [Fact]
        public async Task GetPokemonByNameWithDescriptionTranslated_ReturnNotFoundObjectResultWhenPokemonNotExists()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string? pokemonName = "pokemon_not_exists";
            mockPokemonService.Setup(m => m.GetPokemonByNameAsync(pokemonName, true))
                .ReturnsAsync(ResultModel<PokemonResponseDTO?>.Failure("Pokemon not found", HttpStatusCode.NotFound));

            var response = await pokemonController.GetPokemonByNameWithDescriptionTranslated(pokemonName) as ObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO?>;

            //Assert
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
            Assert.Null(responseValue.Data);
        }

        [Fact]
        public async Task GetPokemonByNameWithDescriptionTranslated_ReturnProblemObjectResultWhenHttpResponseError()
        {
            //Arrange
            var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
            var mockPokemonService = new Mock<IPokemonService>();
            var pokemonController = new PokemonController(mockPokemonService.Object, mockPokemonLogger.Object);

            //Act
            string? pokemonName = "pokemon_not_exists";
            mockPokemonService.Setup(m => m.GetPokemonByNameAsync(pokemonName, true))
                .ReturnsAsync(ResultModel<PokemonResponseDTO?>.Failure("Bad Gateway", HttpStatusCode.BadGateway));

            var response = await pokemonController.GetPokemonByNameWithDescriptionTranslated(pokemonName) as ObjectResult;
            var responseValue = response?.Value as ResultModel<PokemonResponseDTO?>;

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.BadGateway);
            Assert.NotNull(responseValue);
            Assert.False(responseValue.IsSuccess);
            Assert.Null(responseValue.Data);
        }
    }
}
