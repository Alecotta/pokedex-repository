using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.WebApi.Controllers;
using Pokedex.WebApi.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Test.Controllers
{
    public class PokemonControllerTest
    {
        //[Fact]
        //public async Task GetPokemonByName_ReturnBadRequestWhenPokemonNameIsEmpty()
        //{
        //    //Arrange
        //    var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
        //    var pokemonController = new PokemonController(mockPokemonLogger.Object);

        //    //Act
        //    string pokemonName = string.Empty;
        //    var response = await pokemonController.GetPokemonByName(pokemonName) as BadRequestObjectResult;
        //    var responseValue = response?.Value as PokemonResponseDTO<string>;

        //    //Assert
        //    Assert.NotNull(responseValue);
        //    Assert.False(responseValue.Success);
        //}

        //[Fact]
        //public async Task GetPokemonByName_ReturnBadRequestWhenPokemonNameIsNull()
        //{
        //    //Arrange
        //    var mockPokemonLogger = new Mock<ILogger<PokemonController>>();
        //    var pokemonController = new PokemonController(mockPokemonLogger.Object);

        //    //Act
        //    string? pokemonName = null;
        //    var response = await pokemonController.GetPokemonByName(pokemonName) as BadRequestObjectResult;
        //    var responseValue = response?.Value as PokemonResponseDTO<string>;

        //    //Assert
        //    Assert.NotNull(responseValue);
        //    Assert.False(responseValue.Success);
        //}


    }
}
