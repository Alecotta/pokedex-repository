namespace Pokedex.WebApi.Services
{
    public class PokemonService
    {
        public async Task<object> GetPokemonByNameAsync(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            return null;
        }
    }
}
