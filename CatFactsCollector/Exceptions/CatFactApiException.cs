using System.Net;

namespace CatFactsCollector.Exceptions;

public class CatFactApiException : Exception
{
    public CatFactApiException(string message, HttpStatusCode? statusCode = null)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public CatFactApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public HttpStatusCode? StatusCode { get; }
}
