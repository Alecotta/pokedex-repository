using Pokedex.WebApi.Models;

namespace Pokedex.WebApi.Factories
{
    public class CustomHttpClientFactory : ICustomHttpClientFactory
    {
        private readonly IConfiguration _configuration;

        public CustomHttpClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpClient CreateTranslationClient(TranslationType translationType)
        {
            var client = new HttpClient
            {
                BaseAddress = translationType switch
                {
                    TranslationType.SHAKESPEARE => new Uri(_configuration["ExternalApis:ShakespeareTranslator"]),
                    TranslationType.YODA => new Uri(_configuration["ExternalApis:YodaTranslator"]),
                    _ => throw new ArgumentException("Invalid translation type"),
                }
            };
            return client;
        }
    }
}
