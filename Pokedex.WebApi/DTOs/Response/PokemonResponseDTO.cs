namespace Pokedex.WebApi.DTOs.Response
{
    public class PokemonResponseDTO<T>
    {
        public bool Success { get; set; }
        public T? Response { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
