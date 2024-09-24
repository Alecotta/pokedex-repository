using Moq;
using Pokedex.Test.Helpers;
using Pokedex.WebApi.Factories;
using Pokedex.WebApi.Infrastructure.ExternalServices;
using Pokedex.WebApi.Models;
using Pokedex.WebApi.Models.Translation;
using System.Net;

namespace Pokedex.Test.Infrastructure
{
    public class TranslatorServiceTest
    {
        [Fact]
        public async Task GetTraslation_ThrowArgumentNullExceptionWhenDescriptionIsEmpty()
        {
            //Arrange
            string description = string.Empty;
            var mockHttpClientFactory = new Mock<ICustomHttpClientFactory>();
            var translatorService = new TranslatorService(mockHttpClientFactory.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await translatorService.GetTraslation(description, It.IsAny<TranslationType>()));
        }

        [Fact]
        public async Task GetTraslation_ThrowArgumentNullExceptionWhenDescriptionIsNull()
        {
            //Arrange
            string? description = null;
            var mockHttpClientFactory = new Mock<ICustomHttpClientFactory>();
            var translatorService = new TranslatorService(mockHttpClientFactory.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await translatorService.GetTraslation(description, It.IsAny<TranslationType>()));
        }

        [Fact]
        public async Task GetTraslation_ReturnTranslationNotNullAndSuccess()
        {
            //Arrange
            string description = "test_description";
            var shakeSpearTranslatorBaseAddress = "https://api.funtranslations.com/translate/shakespeare.json";

            var mockHttpResponse = new TranslationModel
            {
                Success = new Success
                {
                    Total = 1
                },
                Contents = new Contents
                {
                    Text = "text_test",
                    Translated = "translated_test",
                    Translation = "translation_test"
                }
            };
            var mockHttpMessageHandler = MockHttpMessageHandler.ReturnMockHttpResponse(HttpStatusCode.OK, mockHttpResponse); ;

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(shakeSpearTranslatorBaseAddress)
            };

            var mockHttpClientFactory = new Mock<ICustomHttpClientFactory>();
            mockHttpClientFactory.Setup(m => m.CreateTranslationClient(It.IsAny<TranslationType>()))
                .Returns(mockHttpClient);

            var translatorService = new TranslatorService(mockHttpClientFactory.Object);

            //Act
            var getTraslationResult = await translatorService.GetTraslation(description, It.IsAny<TranslationType>());

            //Assert
            Assert.True(getTraslationResult.IsSuccess);
            Assert.NotNull(getTraslationResult.Data);
        }

        [Fact]
        public async Task GetTraslation_ReturnTranslationNullAndNotSuccessWhenExternalService404()
        {
            //Arrange
            string description = "test_description";
            var shakeSpearTranslatorBaseAddress = "https://api.funtranslations.com/translate/shakespeare.json";

            var mockHttpMessageHandler = MockHttpMessageHandler.ReturnMockHttpResponse<object?>(HttpStatusCode.NotFound, null); ;

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(shakeSpearTranslatorBaseAddress)
            };

            var mockHttpClientFactory = new Mock<ICustomHttpClientFactory>();
            mockHttpClientFactory.Setup(m => m.CreateTranslationClient(It.IsAny<TranslationType>()))
                .Returns(mockHttpClient);

            var translatorService = new TranslatorService(mockHttpClientFactory.Object);

            //Act
            var getTraslationResult = await translatorService.GetTraslation(description, It.IsAny<TranslationType>());

            //Assert
            Assert.False(getTraslationResult.IsSuccess);
            Assert.Equal(getTraslationResult.StatusCode, HttpStatusCode.NotFound);
            Assert.Null(getTraslationResult.Data);
        }

        [Fact]
        public async Task GetTraslation_ReturnTranslationNullAndNotSuccessWhenExternalService502()
        {
            //Arrange
            string description = "test_description";
            var shakeSpearTranslatorBaseAddress = "https://api.funtranslations.com/translate/shakespeare.json";

            var mockHttpMessageHandler = MockHttpMessageHandler.ReturnMockHttpResponse<object?>(HttpStatusCode.BadGateway, null);

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(shakeSpearTranslatorBaseAddress)
            };

            var mockHttpClientFactory = new Mock<ICustomHttpClientFactory>();
            mockHttpClientFactory.Setup(m => m.CreateTranslationClient(It.IsAny<TranslationType>()))
                .Returns(mockHttpClient);

            var translatorService = new TranslatorService(mockHttpClientFactory.Object);

            //Act
            var getTraslationResult = await translatorService.GetTraslation(description, It.IsAny<TranslationType>());

            //Assert
            Assert.False(getTraslationResult.IsSuccess);
            Assert.Equal(getTraslationResult.StatusCode, HttpStatusCode.BadGateway);
            Assert.Null(getTraslationResult.Data);
        }
    }
}
