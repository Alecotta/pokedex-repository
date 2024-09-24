using Pokedex.WebApi.Factories;
using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.Translation;
using System.Transactions;

namespace Pokedex.WebApi.Infrastructure.ExternalServices
{
    public class TranslatorService : ITranslatorService
    {
        private readonly ICustomHttpClientFactory _httpClientFactory;

        public TranslatorService(ICustomHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResultModel<TranslationModel?>> GetTraslation
        (
            string description,
            TranslationType translationType
        )
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            HttpResponseMessage? translationResponse;

            try
            {
                var client = _httpClientFactory.CreateTranslationClient(translationType);
                translationResponse = await client.GetAsync($"?text={description}");
                translationResponse.EnsureSuccessStatusCode();
                
                var translationModel = await translationResponse.Content.ReadFromJsonAsync<TranslationModel>();
                return ResultModel<TranslationModel?>.Success(translationModel);
            }
            catch (Exception ex)
            {
                return ResultModel<TranslationModel?>.Failure(ex.Message, ex.GetType() == typeof(HttpRequestException) ? ((HttpRequestException)ex).StatusCode : null);
            }
        }
    }
}
