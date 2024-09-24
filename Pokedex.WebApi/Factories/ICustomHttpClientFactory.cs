using Pokedex.WebApi.Models;

namespace Pokedex.WebApi.Factories
{
    public interface ICustomHttpClientFactory
    {
        HttpClient CreateTranslationClient(TranslationType translationType);
    }
}