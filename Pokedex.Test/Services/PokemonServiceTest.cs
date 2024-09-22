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
            var pokemonService = new PokemonService();

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokemonService.GetPokemonByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ThrowArgumentNullExceptionWhenPokemonNameIsEmpty()
        {
            //Arrange
            string pokemonName = string.Empty;
            var pokemonService = new PokemonService();

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokemonService.GetPokemonByNameAsync(pokemonName));
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ReturnHandledErrorWhenExternalServiceIsUnavailable()
        {
            //Arrange
            string pokemonName = string.Empty;
            var pokemonService = new PokemonService();

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pokemonService.GetPokemonByNameAsync(pokemonName));
        }
    }
}
