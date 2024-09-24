using System.Text.Json.Serialization;

namespace Pokedex.WebApi.Models.Translation
{
    public class Contents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; } = string.Empty;
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
        [JsonPropertyName("translation")]
        public string Translation { get; set; } = string.Empty;
    }

    public class TranslationModel
    {
        [JsonPropertyName("success")]
        public Success? Success { get; set; }
        [JsonPropertyName("contents")]
        public Contents? Contents { get; set; }
    }

    public class Success
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
