using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using CatFactsCollector.Contracts;
using CatFactsCollector.Exceptions;
using CatFactsCollector.Models;

namespace CatFactsCollector.Services;

public class CatFactService(HttpClient httpClient) : ICatFactService
{
    public async Task<CatFact?> GetCatFactAsync(int? length, CancellationToken cancellationToken = default)
    {
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        
        if (length != null) parameters["max_length"] = length.ToString();

        return await GetFromApiAsync<CatFact>(BuildRelativeUri("fact", parameters.ToString()), cancellationToken);
    }

    public async Task<CatFactsDto?> GetCatFactsAsync(int? length, int? limit, CancellationToken cancellationToken = default)
    {
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        
        if (length != null) parameters["max_length"] = length.ToString();
        if (limit != null) parameters["limit"] = limit.ToString();

        return await GetFromApiAsync<CatFactsDto>(BuildRelativeUri("facts", parameters.ToString()), cancellationToken);
    }

    private async Task<T?> GetFromApiAsync<T>(string uri, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.GetAsync(uri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new CatFactApiException(
                    $"Cat fact API returned {(int)response.StatusCode} {response.ReasonPhrase}.",
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        }
        catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            throw new CatFactApiException("Cat fact API request timed out.", exception);
        }
        catch (HttpRequestException exception)
        {
            throw new CatFactApiException("Cat fact API request failed.", exception);
        }
        catch (JsonException exception)
        {
            throw new CatFactApiException("Cat fact API returned invalid JSON or an unexpected response format.", exception);
        }
    }

    private static string BuildRelativeUri(string path, string? query)
    {
        return string.IsNullOrWhiteSpace(query) ? path : $"{path}?{query}";
    }
}
