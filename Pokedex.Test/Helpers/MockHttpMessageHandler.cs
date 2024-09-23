using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Pokedex.Test.Helpers
{
    public static class MockHttpMessageHandler
    {
        public static Mock<HttpMessageHandler> ReturnMockHttpResponse<T>
        (
            HttpStatusCode httpStatusCode,
            T responseMock
        )
        {
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = responseMock is not null ? new StringContent(JObject.FromObject(responseMock).ToString()) : null
            };

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

            return mockMessageHandler;
        }
    }
}
