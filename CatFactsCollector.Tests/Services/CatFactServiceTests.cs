using System.Net;
using System.Text;
using CatFactsCollector.Services;

namespace CatFactsCollector.Tests;

public class CatFactServiceTests
{
    [Fact]
    public async Task GetCatFactAsync_ReturnsCatFact()
    {
        var httpClient = new HttpClient(new FakeHttpMessageHandler(
            """
            {
                "fact": "Cats have supersonic hearing",
                "length": 28
            }
            """))
        {
            BaseAddress = new Uri("https://catfact.ninja/")
        };

        var service = new CatFactService(httpClient);

        var result = await service.GetCatFactAsync(null);

        Assert.Equal("Cats have supersonic hearing", result!.Fact);
    }

    [Fact]
    public async Task GetCatFactsAsync_ReturnsCatFacts()
    {
        var httpClient = new HttpClient(new FakeHttpMessageHandler(
            """
            {
                "data": [
                    {
                        "fact": "Cats have supersonic hearing",
                        "length": 28
                    }
                ]
            }
            """))
        {
            BaseAddress = new Uri("https://catfact.ninja/")
        };

        var service = new CatFactService(httpClient);

        var result = await service.GetCatFactsAsync(null, null);

        Assert.Equal("Cats have supersonic hearing", result!.Facts![0].Fact);
    }

    private class FakeHttpMessageHandler(string responseContent) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            });
        }
    }
}