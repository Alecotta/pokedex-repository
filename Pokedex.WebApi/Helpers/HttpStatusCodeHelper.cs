using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Pokedex.WebApi.Helpers
{
    public static class HttpStatusCodeHelper
    {
        public static ObjectResult ToObjectResult(HttpStatusCode statusCode, object value)
        {
            return statusCode switch
            {
                HttpStatusCode.OK => new OkObjectResult(value),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(value),
                HttpStatusCode.NotFound => new NotFoundObjectResult(value),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(value),
                HttpStatusCode.Forbidden => new ObjectResult(value) { StatusCode = (int)HttpStatusCode.Forbidden },
                HttpStatusCode.InternalServerError => new ObjectResult(value) { StatusCode = (int)HttpStatusCode.InternalServerError },
                HttpStatusCode.ServiceUnavailable => new ObjectResult(value) { StatusCode = (int)HttpStatusCode.ServiceUnavailable },
                HttpStatusCode.GatewayTimeout => new ObjectResult(value) { StatusCode = (int)HttpStatusCode.GatewayTimeout },
                HttpStatusCode.BadGateway => new ObjectResult(value) { StatusCode = (int)HttpStatusCode.BadGateway },
                _ => new ObjectResult(value) { StatusCode = (int)statusCode }
            };
        }
    }
}
