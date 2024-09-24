using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.Translation;

namespace Pokedex.WebApi.Infrastructure.ExternalServices
{
    public interface ITranslatorService
    {
        Task<ResultModel<TranslationModel?>> GetTraslation(string description, TranslationType translationType);
    }
}